using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class DispositivoService : IDispositivoService
{
    private readonly IDispositivoRepository _dispositivoRepository;

    public DispositivoService(IDispositivoRepository dispositivoRepository)
    {
        _dispositivoRepository = dispositivoRepository;
    }

    public async Task RegistrarAsync(
        Guid usuarioId,
        string token,
        string plataforma)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                "El token del dispositivo es obligatorio.");
        }

        var existente = await _dispositivoRepository
            .ObtenerPorTokenAsync(token.Trim());

        if (existente is not null)
        {
            existente.UsuarioId = usuarioId;
            existente.FechaActualizacion = DateTime.UtcNow;

            await _dispositivoRepository.ActualizarAsync(existente);
            await _dispositivoRepository.GuardarCambiosAsync();

            return;
        }

        var dispositivo = new Dispositivo
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            Token = token.Trim(),
            Plataforma = string.IsNullOrWhiteSpace(plataforma)
                ? "android"
                : plataforma.Trim().ToLowerInvariant(),
            FechaRegistro = DateTime.UtcNow,
            FechaActualizacion = DateTime.UtcNow
        };

        await _dispositivoRepository.CrearAsync(dispositivo);
        await _dispositivoRepository.GuardarCambiosAsync();
    }
}
