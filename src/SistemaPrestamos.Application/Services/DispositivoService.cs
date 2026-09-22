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

        // Tope: máximo 5 dispositivos por usuario (los más recientes).
        var todos = (await _dispositivoRepository
            .ObtenerPorUsuarioAsync(usuarioId))
            .OrderByDescending(x => x.FechaActualizacion)
            .Skip(5)
            .ToList();

        foreach (var viejo in todos)
        {
            await _dispositivoRepository.EliminarAsync(viejo);
        }

        if (todos.Count > 0)
        {
            await _dispositivoRepository.GuardarCambiosAsync();
        }
    }

    public async Task<bool> EliminarAsync(Guid id)
    {
        var dispositivo = await _dispositivoRepository
            .ObtenerPorIdAsync(id);

        if (dispositivo is null)
        {
            return false;
        }

        await _dispositivoRepository.EliminarAsync(dispositivo);
        await _dispositivoRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> EliminarPorTokenAsync(
        Guid usuarioId,
        string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var dispositivo = await _dispositivoRepository
            .ObtenerPorTokenAsync(token.Trim());

        if (dispositivo is null || dispositivo.UsuarioId != usuarioId)
        {
            return false;
        }

        await _dispositivoRepository.EliminarAsync(dispositivo);
        await _dispositivoRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<IEnumerable<Dispositivo>> ObtenerTodosAsync()
    {
        return await _dispositivoRepository.ObtenerTodosAsync();
    }
}
