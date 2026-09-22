using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IDispositivoService
{
    Task RegistrarAsync(Guid usuarioId, string token, string plataforma);

    Task<bool> EliminarAsync(Guid id);

    Task<bool> EliminarPorTokenAsync(Guid usuarioId, string token);

    Task<IEnumerable<Dispositivo>> ObtenerTodosAsync();
}
