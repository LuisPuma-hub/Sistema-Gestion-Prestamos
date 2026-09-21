using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class MorosidadService : IMorosidadService
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IPeriodoInteresRepository _periodoInteresRepository;
    private readonly IMorosidadRepository _morosidadRepository;
    private readonly IPeriodoInteresService _periodoInteresService;

    public MorosidadService(
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoInteresRepository,
        IMorosidadRepository morosidadRepository,
        IPeriodoInteresService periodoInteresService)
    {
        _prestamoRepository = prestamoRepository;
        _periodoInteresRepository = periodoInteresRepository;
        _morosidadRepository = morosidadRepository;
        _periodoInteresService = periodoInteresService;
    }

    public async Task<MorosidadDto?> ObtenerPorPrestamoAsync(
        Guid prestamoId)
    {
        var prestamo = await _prestamoRepository
            .ObtenerPorIdAsync(prestamoId);

        if (prestamo is null)
            throw new InvalidOperationException(
                "El préstamo no existe.");

        var morosidad = await _morosidadRepository
            .ObtenerPorPrestamoAsync(prestamoId);

        if (morosidad is null)
            return null;

        return MapearDto(morosidad);
    }

    public async Task<MorosidadDto> EvaluarAsync(
        Guid prestamoId,
        DateTime fechaReferencia)
    {
        var prestamo = await _prestamoRepository
            .ObtenerPorIdAsync(prestamoId);

        if (prestamo is null)
            throw new InvalidOperationException(
                "El préstamo no existe.");

        // Normalizar a UTC (la referencia puede llegar sin zona)
        fechaReferencia = fechaReferencia == default
            ? DateTime.UtcNow
            : DateTime.SpecifyKind(fechaReferencia, DateTimeKind.Utc);

        // Generar los períodos vencidos hasta la referencia para
        // que la mora refleje las semanas transcurridas aunque
        // no se hayan registrado pagos.
        if (prestamo.Estado == "Activo")
        {
            await _periodoInteresService.GenerarPeriodosPendientesAsync(
                prestamoId,
                fechaReferencia);
        }

        var periodos = await _periodoInteresRepository
            .ObtenerPorPrestamoAsync(prestamoId);

        var periodosVencidos = periodos
            .Where(x =>
                x.FechaVencimiento < fechaReferencia &&
                x.InteresPendiente > 0)
            .OrderBy(x => x.FechaVencimiento)
            .ToList();

        var cantidadVencidos = periodosVencidos.Count;

        var morosidad = await _morosidadRepository
            .ObtenerPorPrestamoAsync(prestamoId);

        // Regla: más de 2 períodos vencidos
        // equivale a 3 o más períodos.
        if (cantidadVencidos >= 3)
        {
            if (morosidad is null)
            {
                morosidad = new Morosidad
                {
                    Id = Guid.NewGuid(),
                    PrestamoId = prestamoId,
                    PagosInteresVencidos = cantidadVencidos,
                    FechaInicio = fechaReferencia,
                    FechaReactivacion = null,
                    Activa = true,
                    Observaciones = null
                };

                await _morosidadRepository
                    .CrearAsync(morosidad);
            }
            else
            {
                morosidad.PagosInteresVencidos =
                    cantidadVencidos;

                morosidad.Activa = true;
                morosidad.FechaReactivacion = null;

                await _morosidadRepository
                    .ActualizarAsync(morosidad);
            }
        }
        else if (morosidad is not null)
        {
            morosidad.PagosInteresVencidos =
                cantidadVencidos;

            await _morosidadRepository
                .ActualizarAsync(morosidad);
        }
        else
        {
            morosidad = new Morosidad
            {
                Id = Guid.NewGuid(),
                PrestamoId = prestamoId,
                PagosInteresVencidos = cantidadVencidos,
                FechaInicio = null,
                FechaReactivacion = null,
                Activa = false,
                Observaciones = null
            };

            await _morosidadRepository
                .CrearAsync(morosidad);
        }

        await _morosidadRepository.GuardarCambiosAsync();

        return MapearDto(morosidad);
    }

    public async Task<MorosidadDto> ReactivarAsync(
        Guid prestamoId,
        string? observaciones)
    {
        var prestamo = await _prestamoRepository
            .ObtenerPorIdAsync(prestamoId);

        if (prestamo is null)
            throw new InvalidOperationException(
                "El préstamo no existe.");

        var morosidad = await _morosidadRepository
            .ObtenerPorPrestamoAsync(prestamoId);

        if (morosidad is null)
            throw new InvalidOperationException(
                "El préstamo no tiene un registro de morosidad.");

        if (!morosidad.Activa)
            throw new InvalidOperationException(
                "El préstamo ya se encuentra reactivado.");

        morosidad.Activa = false;
        morosidad.FechaReactivacion = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(observaciones))
        {
            morosidad.Observaciones = observaciones;
        }

        await _morosidadRepository
            .ActualizarAsync(morosidad);

        await _morosidadRepository
            .GuardarCambiosAsync();

        return MapearDto(morosidad);
    }

    private static MorosidadDto MapearDto(
        Morosidad morosidad)
    {
        return new MorosidadDto
        {
            Id = morosidad.Id,
            PrestamoId = morosidad.PrestamoId,
            PagosInteresVencidos =
                morosidad.PagosInteresVencidos,
            FechaInicio = morosidad.FechaInicio,
            FechaReactivacion =
                morosidad.FechaReactivacion,
            Activa = morosidad.Activa,
            Observaciones =
                morosidad.Observaciones
        };
    }
}