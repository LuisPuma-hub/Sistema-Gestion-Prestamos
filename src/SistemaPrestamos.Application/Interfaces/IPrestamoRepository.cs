using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IPrestamoRepository
{
    Task<IEnumerable<Prestamo>> ObtenerTodosAsync();

    Task<Prestamo?> ObtenerPorIdAsync(Guid id);

    Task<IEnumerable<Prestamo>> ObtenerPorClienteAsync(Guid clienteId);

    Task<IEnumerable<Prestamo>> ObtenerActivosAsync();

    Task<Prestamo> CrearAsync(Prestamo prestamo);

    Task ActualizarAsync(Prestamo prestamo);

    Task EliminarAsync(Prestamo prestamo);

    Task<int> EliminarPorClienteAsync(Guid clienteId);

    Task GuardarCambiosAsync();
}