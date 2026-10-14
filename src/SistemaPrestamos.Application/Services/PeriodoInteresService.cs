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

        if (prestamo.Estado != "Activo" &&
            prestamo.Estado != "Moroso")
            throw new InvalidOperationException(
                "Solo se pueden generar períodos para préstamos " +
                "activos o morosos.");

        // PlazoFijo: las N cuotas se pre-generan al aprobar. Solo
        // falta el retorno semanal si venció el plazo con saldo.
        if (EsPlazoFijo(prestamo))
        {
            await GenerarSemanalesPostPlazoAsync(
                prestamo, fechaReferencia);

            return;
        }

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

    /// <summary>
    /// Retorno semanal: vencido el plazo fijo con saldo pendiente,
    /// se generan tramos de 7 días al 5% sobre el saldo (RN-PRE-019).
    /// </summary>
    public async Task GenerarSemanalesPostPlazoAsync(
        Guid prestamoId,
        DateTime fechaReferencia)
    {
        var prestamo =
            await _prestamoRepository.ObtenerPorIdAsync(prestamoId);

        if (prestamo is null || !EsPlazoFijo(prestamo))
            return;

        await GenerarSemanalesPostPlazoAsync(
            prestamo, fechaReferencia);
    }

    private async Task GenerarSemanalesPostPlazoAsync(
        Prestamo prestamo,
        DateTime fechaReferencia)
    {
        if (prestamo.FechaFin is null ||
            fechaReferencia.Date <= prestamo.FechaFin.Value.Date)
            return;

        if (prestamo.CapitalPendiente <= 0)
            return;

        var periodos =
            (await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamo.Id))
            .OrderBy(x => x.FechaInicio)
            .ToList();

        var inicio = periodos.Any()
            ? periodos.Last().FechaVencimiento
            : prestamo.FechaFin.Value;

        var generados = new HashSet<DateTime>(
            periodos.Select(x => x.FechaInicio));

        while (inicio <= fechaReferencia)
        {
            if (!generados.Contains(inicio))
            {
                var interes = Math.Round(
                    prestamo.CapitalPendiente *
                        prestamo.TasaInteresSemanal,
                    2,
                    MidpointRounding.AwayFromZero);

                await _periodoInteresRepository.CrearAsync(
                    new PeriodoInteres
                    {
                        Id = Guid.NewGuid(),
                        PrestamoId = prestamo.Id,
                        FechaInicio = inicio,
                        FechaVencimiento = inicio.AddDays(7),
                        InteresGenerado = interes,
                        InteresPagado = 0,
                        InteresPendiente = interes,
                    Estado = "Pendiente",
                    FechaPagoCompleto = null,
                    EsCuotaFija = false,
                    FechaRegistro = DateTime.UtcNow
                });

                generados.Add(inicio);
            }

            inicio = inicio.AddDays(7);
        }

        await _periodoInteresRepository.GuardarCambiosAsync();
    }

    public static bool EsPlazoFijo(Prestamo prestamo)
    {
        return string.Equals(
            prestamo.ModalidadCobro,
            "PlazoFijo",
            StringComparison.OrdinalIgnoreCase);
    }

    public static bool EsFrecuenciaDiaria(Prestamo prestamo)
    {
        return string.Equals(
            prestamo.FrecuenciaCuota,
            "Diaria",
            StringComparison.OrdinalIgnoreCase);
    }

    public static bool EsDomingoLima(DateTime fechaUtc)
    {
        return ProgramadorService.AhoraLima(
            DateTime.SpecifyKind(fechaUtc, DateTimeKind.Utc))
            .DayOfWeek == DayOfWeek.Sunday;
    }

    /// <summary>
    /// Avanza al siguiente día de cobro (los domingos no se
    /// cobran en modalidad Diaria).
    /// </summary>
    public static DateTime SiguienteDiaCobro(DateTime fecha)
    {
        var dia = fecha.Date;

        while (EsDomingoLima(dia))
            dia = dia.AddDays(1);

        return dia;
    }

    /// <summary>
    /// Plan diario: interés total = capital x 5% x (N/7),
    /// cuota redondeada siempre hacia arriba al décimo
    /// (13.13 -> 13.20). La última absorbe la diferencia.
    /// </summary>
    public static (decimal InteresTotal, decimal MontoTotal, decimal Cuota)
        CalcularPlanDiario(
            decimal capital,
            decimal tasaSemanal,
            int cuotas)
    {
        var interesTotal = Math.Round(
            capital * tasaSemanal * cuotas / 7m,
            2,
            MidpointRounding.AwayFromZero);

        var montoTotal = capital + interesTotal;

        var cuota = Math.Ceiling(montoTotal / cuotas * 10m) / 10m;

        return (interesTotal, montoTotal, cuota);
    }

    /// <summary>
    /// Cuotas del plazo fijo. Diaria: días Lun-Sáb (sin
    /// domingos). Semanal: cada 7 días (si cae domingo se
    /// pasa al lunes). Interés repartido, la última absorbe.
    /// </summary>
    public static List<(DateTime Inicio, DateTime Vencimiento, decimal Interes)>
        ArmarCuotasPlazo(
            DateTime fechaInicio,
            int cuotas,
            decimal interesTotal,
            bool frecuenciaDiaria)
    {
        var lista = new List<(DateTime, DateTime, decimal)>();

        var interesBase = Math.Round(
            interesTotal / cuotas,
            2,
            MidpointRounding.AwayFromZero);

        var acumulado = 0m;

        var dia = frecuenciaDiaria
            ? SiguienteDiaCobro(fechaInicio)
            : fechaInicio.Date;

        for (var i = 0; i < cuotas; i++)
        {
            var interes = i == cuotas - 1
                ? interesTotal - acumulado
                : interesBase;

            if (i < cuotas - 1)
                acumulado += interes;

            var siguiente = frecuenciaDiaria
                ? SiguienteDiaCobro(dia.AddDays(1))
                : SiguienteDiaCobro(dia.AddDays(7));

            lista.Add((dia, siguiente, interes));

            dia = siguiente;
        }

        return lista;
    }

    /// <summary>
    /// Cuotas diarias Lun-Sáb con interés repartido (la última
    /// absorbe el redondeo). Sin cuota dominical.
    /// </summary>
    public static List<(DateTime Inicio, DateTime Vencimiento, decimal Interes)>
        ArmarCuotasDiarias(
            DateTime fechaInicio,
            int cuotas,
            decimal interesTotal)
    {
        return ArmarCuotasPlazo(
            fechaInicio, cuotas, interesTotal, true);
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

        var interesGenerado = CalcularInteres(prestamo);

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
            EsCuotaFija = false,
            FechaRegistro = DateTime.UtcNow
        };

        await _periodoInteresRepository.CrearAsync(periodo);
    }

    private static decimal CalcularInteres(Prestamo prestamo)
    {
        var esSaldo = string.Equals(
            prestamo.EsquemaInteres,
            "Saldo",
            StringComparison.OrdinalIgnoreCase);

        var baseCalculo = esSaldo
            ? prestamo.CapitalPendiente
            : prestamo.CapitalInicial;

        return Math.Round(
            baseCalculo * prestamo.TasaInteresSemanal,
            2,
            MidpointRounding.AwayFromZero);
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
            EsCuotaFija = periodo.EsCuotaFija,
            FechaRegistro = periodo.FechaRegistro
        };
    }
}