using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IMorosidadService
{
    Task<MorosidadDto?> ObtenerPorPrestamoAsync(Guid prestamoId);

    Task<MorosidadDto> EvaluarAsync(
        Guid prestamoId,
        DateTime fechaReferencia);

    Task<MorosidadDto> ReactivarAsync(
        Guid prestamoId,
        string? observaciones);
}