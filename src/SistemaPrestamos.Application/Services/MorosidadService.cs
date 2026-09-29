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

        // RN-MOR-005 + RN-MOR-009: el préstamo con 3 o más
        // periodos vencidos pasa a MOROSO y arrastra al cliente.
        // Sin esto el cliente seguía ACTIVO y podía recibir
        // otro préstamo.
        if (!EsTerminal(prestamo.Estado))
        {
            if (cantidadVencidos >= 3)
            {
                if (string.Equals(
                        prestamo.Estado,
                        "Activo",
                        StringComparison.OrdinalIgnoreCase))
                    prestamo.Estado = "Moroso";

                if (prestamo.Cliente is not null &&
                    !string.Equals(
                        prestamo.Cliente.Estado,
                        "Moroso",
                        StringComparison.OrdinalIgnoreCase))
                    prestamo.Cliente.Estado = "Moroso";
            }
            else if (prestamo.Cliente is not null)
            {
                // El préstamo solo vuelve a ACTIVO por vía manual
                // (RN-PRE-012); aquí solo se sincroniza al cliente.
                await SincronizarClienteAsync(prestamo.Cliente);
            }
        }
        else
        {
            // Préstamo terminal: cerrar mora activa residual.
            if (morosidad is not null && morosidad.Activa)
            {
                morosidad.Activa = false;
                morosidad.FechaReactivacion = fechaReferencia;

                await _morosidadRepository
                    .ActualizarAsync(morosidad);
            }

            if (prestamo.Cliente is not null)
                await SincronizarClienteAsync(prestamo.Cliente);
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

        // RN-PRE-012: el préstamo vuelve a ACTIVO por vía manual.
        if (string.Equals(
                prestamo.Estado,
                "Moroso",
                StringComparison.OrdinalIgnoreCase))
            prestamo.Estado = "Activo";

        morosidad.Activa = false;
        morosidad.FechaReactivacion = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(observaciones))
        {
            morosidad.Observaciones = observaciones;
        }

        await _morosidadRepository
            .ActualizarAsync(morosidad);

        // RN-MOR-009: el cliente vuelve a ACTIVO solo si ya no
        // tiene préstamos en MOROSO (este ya va a ACTIVO).
        if (prestamo.Cliente is not null)
            await SincronizarClienteAsync(prestamo.Cliente, prestamo.Id);

        await _morosidadRepository
            .GuardarCambiosAsync();

        return MapearDto(morosidad);
    }

    public async Task<int> EvaluarVigentesAsync(
        DateTime fechaReferencia)
    {
        var activos = await _prestamoRepository.ObtenerActivosAsync();

        var enMora = await _morosidadRepository.ObtenerActivasAsync();

        var ids = activos
            .Select(x => x.Id)
            .Concat(enMora.Select(x => x.PrestamoId))
            .Distinct()
            .ToList();

        foreach (var id in ids)
        {
            await EvaluarAsync(id, fechaReferencia);
        }

        return ids.Count;
    }

    private async Task SincronizarClienteAsync(
        Cliente cliente,
        Guid? ignorarPrestamoId = null)
    {
        var prestamos = await _prestamoRepository
            .ObtenerPorClienteAsync(cliente.Id);

        // La lectura es sin seguimiento: el préstamo en transición
        // aún figura con su estado anterior, por eso se excluye
        // (ya se sabe su estado final).
        var hayMorosos = prestamos.Any(x =>
            (ignorarPrestamoId is null || x.Id != ignorarPrestamoId) &&
            string.Equals(
                x.Estado,
                "Moroso",
                StringComparison.OrdinalIgnoreCase));

        if (hayMorosos &&
            !string.Equals(
                cliente.Estado,
                "Moroso",
                StringComparison.OrdinalIgnoreCase))
        {
            cliente.Estado = "Moroso";
        }
        else if (!hayMorosos &&
            string.Equals(
                cliente.Estado,
                "Moroso",
                StringComparison.OrdinalIgnoreCase))
        {
            // Sale de mora bajo observación: el pase a
            // Activo lo hace el administrador manualmente.
            cliente.Estado = "En observación";
        }
    }

    private static bool EsTerminal(string estado)
    {
        return string.Equals(
                estado,
                "Cancelado",
                StringComparison.OrdinalIgnoreCase) ||
            string.Equals(
                estado,
                "Anulado",
                StringComparison.OrdinalIgnoreCase);
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