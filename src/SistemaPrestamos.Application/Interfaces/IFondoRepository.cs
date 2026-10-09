using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IFondoRepository
{
    Task<IEnumerable<FondoMovimiento>> ObtenerTodosAsync();

    Task<FondoMovimiento?> ObtenerPorIdAsync(Guid id);

    Task<FondoMovimiento> CrearAsync(FondoMovimiento movimiento);

    Task EliminarAsync(FondoMovimiento movimiento);

    Task GuardarCambiosAsync();
}
