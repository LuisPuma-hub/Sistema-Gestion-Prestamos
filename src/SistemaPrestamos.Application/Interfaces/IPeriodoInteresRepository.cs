using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IPeriodoInteresRepository
{
    Task<PeriodoInteres> CrearAsync(PeriodoInteres periodo);

    Task<IEnumerable<PeriodoInteres>> ObtenerPorPrestamoAsync(
        Guid prestamoId);

    Task<IEnumerable<PeriodoInteres>> ObtenerConVencimientoAsync(
        DateTime fecha);

    Task<PeriodoInteres?> ObtenerPorIdAsync(Guid id);

    Task EliminarAsync(PeriodoInteres periodo);

    Task<int> EliminarPorPrestamoAsync(Guid prestamoId);

    Task GuardarCambiosAsync();
}