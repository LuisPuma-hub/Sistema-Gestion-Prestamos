using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IDispositivoRepository
{
    Task<Dispositivo?> ObtenerPorTokenAsync(string token);

    Task<Dispositivo?> ObtenerPorIdAsync(Guid id);

    Task<IEnumerable<Dispositivo>> ObtenerPorUsuarioAsync(Guid usuarioId);

    Task<IEnumerable<Dispositivo>> ObtenerTodosAsync();

    Task<Dispositivo> CrearAsync(Dispositivo dispositivo);

    Task ActualizarAsync(Dispositivo dispositivo);

    Task EliminarAsync(Dispositivo dispositivo);

    Task GuardarCambiosAsync();
}
