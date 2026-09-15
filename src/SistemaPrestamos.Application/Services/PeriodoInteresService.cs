using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class PeriodoInteresService : IPeriodoInteresService
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IPeriodoInteresRepository _periodoInteresRepository;

    public PeriodoInteresService(
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoInteresRepository)
    {
        _prestamoRepository = prestamoRepository;
        _periodoInteresRepository = periodoInteresRepository;
    }

    public async Task GenerarPeriodosPendientesAsync(
        Guid prestamoId,
        DateTime fechaReferencia)
    {
        var prestamo = await _prestamoRepository.ObtenerPorIdAsync(prestamoId);

        if (prestamo is null)
            throw new InvalidOperationException("El préstamo no existe.");

        if (prestamo.Estado != "Activo")
            throw new InvalidOperationException(
                "Solo se pueden generar períodos para préstamos activos.");

        var periodos =
            (await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamoId))
            .OrderBy(x => x.FechaInicio)
            .ToList();

        if (!periodos.Any())
        {
            await CrearPeriodoAsync(
                prestamo,
                prestamo.FechaInicio);

            await _periodoInteresRepository.GuardarCambiosAsync();

            return;
        }

        var ultimoPeriodo = periodos.Last();

        var siguienteInicio = ultimoPeriodo.FechaVencimiento;

        while (siguienteInicio <= fechaReferencia)
        {
            await CrearPeriodoAsync(
                prestamo,
                siguienteInicio);

            siguienteInicio = siguienteInicio.AddDays(7);
        }

        await _periodoInteresRepository.GuardarCambiosAsync();
    }

    public async Task<IEnumerable<PeriodoInteresDto>> ObtenerPorPrestamoAsync(
        Guid prestamoId)
    {
        var prestamo = await _prestamoRepository
            .ObtenerPorIdAsync(prestamoId);

        if (prestamo is null)
            throw new InvalidOperationException(
                "El préstamo no existe.");

        var periodos = await _periodoInteresRepository
            .ObtenerPorPrestamoAsync(prestamoId);

        return periodos
            .OrderBy(x => x.FechaInicio)
            .Select(MapearDto)
            .ToList();
    }

    private async Task CrearPeriodoAsync(
        Prestamo prestamo,
        DateTime fechaInicio)
    {
        var periodos =
            await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamo.Id);

        if (periodos.Any(x => x.FechaInicio == fechaInicio))
            return;

        var interesGenerado =
            prestamo.CapitalInicial *
            prestamo.TasaInteresSemanal;

        var periodo = new PeriodoInteres
        {
            Id = Guid.NewGuid(),
            PrestamoId = prestamo.Id,
            FechaInicio = fechaInicio,
            FechaVencimiento = fechaInicio.AddDays(7),
            InteresGenerado = interesGenerado,
            InteresPagado = 0,
            InteresPendiente = interesGenerado,
            Estado = "Pendiente",
            FechaPagoCompleto = null,
            FechaRegistro = DateTime.UtcNow
        };

        await _periodoInteresRepository.CrearAsync(periodo);
    }

    private static PeriodoInteresDto MapearDto(
        PeriodoInteres periodo)
    {
        return new PeriodoInteresDto
        {
            Id = periodo.Id,
            PrestamoId = periodo.PrestamoId,
            FechaInicio = periodo.FechaInicio,
            FechaVencimiento = periodo.FechaVencimiento,
            InteresGenerado = periodo.InteresGenerado,
            InteresPagado = periodo.InteresPagado,
            InteresPendiente = periodo.InteresPendiente,
            Estado = periodo.Estado,
            FechaPagoCompleto = periodo.FechaPagoCompleto,
            FechaRegistro = periodo.FechaRegistro
        };
    }
}