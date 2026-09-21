using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IDispositivoRepository
{
    Task<Dispositivo?> ObtenerPorTokenAsync(string token);

    Task<IEnumerable<Dispositivo>> ObtenerPorUsuarioAsync(Guid usuarioId);

    Task<Dispositivo> CrearAsync(Dispositivo dispositivo);

    Task ActualizarAsync(Dispositivo dispositivo);

    Task GuardarCambiosAsync();
}
