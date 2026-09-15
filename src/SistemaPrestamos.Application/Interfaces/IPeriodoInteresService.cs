using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IPeriodoInteresService
{
    Task GenerarPeriodosPendientesAsync(
        Guid prestamoId,
        DateTime fechaReferencia);

    Task<IEnumerable<PeriodoInteresDto>> ObtenerPorPrestamoAsync(
        Guid prestamoId);
}