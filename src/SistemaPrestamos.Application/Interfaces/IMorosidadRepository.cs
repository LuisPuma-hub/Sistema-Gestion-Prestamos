using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IMorosidadRepository
{
    Task<Morosidad?> ObtenerPorPrestamoAsync(Guid prestamoId);

    Task<Morosidad?> ObtenerPorIdAsync(Guid id);

    Task<Morosidad> CrearAsync(Morosidad morosidad);

    Task ActualizarAsync(Morosidad morosidad);

    Task EliminarAsync(Morosidad morosidad);

    Task<int> EliminarPorPrestamoAsync(Guid prestamoId);

    Task GuardarCambiosAsync();
}