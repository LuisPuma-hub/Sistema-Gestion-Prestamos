using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IPagoRepository
{
    Task<IEnumerable<Pago>> ObtenerTodosAsync();
    Task<Pago?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<Pago>> ObtenerPorPrestamoAsync(Guid prestamoId);
    Task<Pago> CrearAsync(Pago pago);
    Task GuardarCambiosAsync();
}