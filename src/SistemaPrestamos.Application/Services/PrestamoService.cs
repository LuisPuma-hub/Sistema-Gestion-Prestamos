using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class PrestamoService : IPrestamoService
{
    private const decimal TasaInteresSemanal = 0.05m;

    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPeriodoInteresRepository _periodoInteresRepository;
    private readonly IPagoRepository _pagoRepository;
    private readonly IWhatsappService? _whatsappService;
    private readonly IPeriodoInteresService? _periodoInteresService;

    public PrestamoService(
        IPrestamoRepository prestamoRepository,
        IClienteRepository clienteRepository,
        IPeriodoInteresRepository periodoInteresRepository,
        IPagoRepository pagoRepository,
        IWhatsappService? whatsappService = null,
        IPeriodoInteresService? periodoInteresService = null)
    {
        _prestamoRepository = prestamoRepository;
        _clienteRepository = clienteRepository;
        _periodoInteresRepository = periodoInteresRepository;
        _pagoRepository = pagoRepository;
        _whatsappService = whatsappService;
        _periodoInteresService = periodoInteresService;
    }

    public async Task<IEnumerable<PrestamoDto>> ObtenerTodosAsync()
    {
        var prestamos = await _prestamoRepository.ObtenerTodosAsync();

        return prestamos.Select(MapearDto);
    }

    public async Task<PrestamoDto?> ObtenerPorIdAsync(Guid id)
    {
        var prestamo = await _prestamoRepository.ObtenerPorIdAsync(id);

        return prestamo is null
            ? null
            : MapearDto(prestamo);
    }

    public async Task<IEnumerable<PrestamoDto>> ObtenerPorClienteAsync(
        Guid clienteId)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);

        if (cliente is null)
            throw new InvalidOperationException(
                "El cliente no existe.");

        var prestamos =
            await _prestamoRepository.ObtenerPorClienteAsync(clienteId);

        return prestamos.Select(MapearDto);
    }

    public async Task<PrestamoDto> CrearAsync(CrearPrestamoDto dto)
    {
        if (dto.ClienteId == Guid.Empty)
            throw new InvalidOperationException(
                "El cliente es obligatorio.");

        if (dto.CapitalInicial <= 0)
            throw new InvalidOperationException(
                "El capital inicial debe ser mayor que cero.");

        var esquema = string.IsNullOrWhiteSpace(dto.EsquemaInteres)
            ? "Saldo"
            : dto.EsquemaInteres.Trim();

        if (!string.Equals(
                esquema, "Fijo",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(
                esquema, "Saldo",
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "El esquema de interés debe ser Fijo o Saldo.");

        esquema = string.Equals(
            esquema, "Fijo",
            StringComparison.OrdinalIgnoreCase)
                ? "Fijo"
                : "Saldo";

        var modalidad = string.IsNullOrWhiteSpace(dto.ModalidadCobro)
            ? "Semanal"
            : dto.ModalidadCobro.Trim();

        if (!string.Equals(
                modalidad, "Semanal",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(
                modalidad, "PlazoFijo",
                StringComparison.OrdinalIgnoreCase) &&
            // Compat: la primera versión usaba "Diaria".
            !string.Equals(
                modalidad, "Diaria",
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "La modalidad debe ser Semanal o PlazoFijo.");

        modalidad = string.Equals(
            modalidad, "Semanal",
            StringComparison.OrdinalIgnoreCase)
                ? "Semanal"
                : "PlazoFijo";

        var frecuencia = string.IsNullOrWhiteSpace(dto.FrecuenciaCuota)
            ? "Diaria"
            : dto.FrecuenciaCuota.Trim();

        if (!string.Equals(
                frecuencia, "Diaria",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(
                frecuencia, "Semanal",
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "La frecuencia debe ser Diaria o Semanal.");

        frecuencia = string.Equals(
            frecuencia, "Semanal",
            StringComparison.OrdinalIgnoreCase)
                ? "Semanal"
                : "Diaria";

        int? plazoCuotas = null;

        if (modalidad == "PlazoFijo")
        {
            if (dto.PlazoValor is null || dto.PlazoValor <= 0)
                throw new InvalidOperationException(
                    "El plazo fijo requiere el número de " +
                    "días o semanas del plazo.");

            if (frecuencia == "Semanal")
            {
                if (!string.Equals(
                        dto.PlazoUnidad, "Semanas",
                        StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException(
                        "Con frecuencia semanal el plazo se " +
                        "indica en semanas.");

                if (dto.PlazoValor > 52)
                    throw new InvalidOperationException(
                        "El plazo máximo es 52 semanas.");

                plazoCuotas = dto.PlazoValor.Value;
            }
            else
            {
                var unidad = string.IsNullOrWhiteSpace(dto.PlazoUnidad)
                    ? "Dias"
                    : dto.PlazoUnidad.Trim();

                if (string.Equals(
                        unidad, "Semanas",
                        StringComparison.OrdinalIgnoreCase))
                {
                    if (dto.PlazoValor > 52)
                        throw new InvalidOperationException(
                            "El plazo máximo es 52 semanas.");

                    plazoCuotas = dto.PlazoValor.Value * 7;
                }
                else if (string.Equals(
                        unidad, "Dias",
                        StringComparison.OrdinalIgnoreCase))
                {
                    if (dto.PlazoValor > 365)
                        throw new InvalidOperationException(
                            "El plazo máximo es 365 días.");

                    plazoCuotas = dto.PlazoValor.Value;
                }
                else
                {
                    throw new InvalidOperationException(
                        "La unidad del plazo debe ser Dias o Semanas.");
                }
            }
        }

        var cliente =
            await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);

        if (cliente is null)
            throw new InvalidOperationException(
                "El cliente no existe.");

        if (cliente.Estado != "Activo")
            throw new InvalidOperationException(
                "Solo se pueden registrar préstamos para clientes " +
                "activos. Un cliente en mora o en observación debe " +
                "regularizar su situación primero.");

        // RN-CLI-008: con cliente Activo se permiten varios
        // préstamos vigentes; el freno es el estado (mora),
        // no la cantidad.

        var fechaInicio = dto.FechaInicio == default
            ? DateTime.UtcNow
            : DateTime.SpecifyKind(dto.FechaInicio, DateTimeKind.Utc);

        var prestamo = new Prestamo
        {
            Id = Guid.NewGuid(),
            ClienteId = dto.ClienteId,
            GaranteId = dto.GaranteId,
            CapitalInicial = dto.CapitalInicial,
            TasaInteresSemanal = TasaInteresSemanal,
            EsquemaInteres = esquema,
            ModalidadCobro = modalidad,
            FrecuenciaCuota = frecuencia,
            PlazoCuotas = plazoCuotas,
            FechaFin = null,
            MontoTotal = 0,
            CuotaFija = 0,
            CapitalPendiente = dto.CapitalInicial,
            FechaInicio = fechaInicio,
            FechaAprobacion = null,
            Estado = "Pendiente"
        };

        await _prestamoRepository.CrearAsync(prestamo);
        await _prestamoRepository.GuardarCambiosAsync();

        var prestamoGuardado =
            await _prestamoRepository.ObtenerPorIdAsync(prestamo.Id);

        if (prestamoGuardado is null)
            throw new InvalidOperationException(
                "No se pudo recuperar el préstamo creado.");

        return MapearDto(prestamoGuardado);
    }

    public async Task<bool> AprobarAsync(Guid id)
    {
        var prestamo =
            await _prestamoRepository.ObtenerPorIdAsync(id);

        if (prestamo is null)
            return false;

        if (prestamo.Estado != "Pendiente")
            throw new InvalidOperationException(
                "Solo se pueden aprobar préstamos pendientes.");

        // Un cliente en mora (o inactivo) no puede activar
        // otro préstamo hasta regularizar (RN-MOR-009).
        var estadoCliente = prestamo.Cliente?.Estado ?? "Activo";

        if (!string.Equals(
                estadoCliente.Trim(),
                "Activo",
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "El cliente no está activo " +
                $"(estado: {estadoCliente}). Regularice su " +
                "situación antes de aprobar.");

        prestamo.Estado = "Activo";
        prestamo.FechaAprobacion = DateTime.UtcNow;

        await _prestamoRepository.ActualizarAsync(prestamo);

        if (PeriodoInteresService.EsPlazoFijo(prestamo))
            await CrearCuotasPlazoAsync(prestamo);
        else
            // Crear el primer período semanal.
            await CrearSiguientePeriodoAsync(
                prestamo,
                prestamo.FechaInicio);

        await _periodoInteresRepository.GuardarCambiosAsync();

        // Aviso automático al cliente (no bloquea la aprobación).
        if (_whatsappService is not null)
        {
            try
            {
                await _whatsappService.EnviarPlantillaCatalogoAsync(
                    prestamo.ClienteId,
                    prestamo.Id,
                    "prestamo_aprobado");
            }
            catch
            {
            }
        }

        return true;
    }

    public async Task<bool> AnularAsync(Guid id, string motivo)
    {
        var prestamo =
            await _prestamoRepository.ObtenerPorIdAsync(id);

        if (prestamo is null)
            return false;

        if (prestamo.Estado != "Pendiente" &&
            prestamo.Estado != "Activo")
            throw new InvalidOperationException(
                "Solo se pueden anular préstamos pendientes o activos.");

        if (string.IsNullOrWhiteSpace(motivo) ||
            motivo.Trim().Length < 10 ||
            motivo.Trim().Length > 200)
            throw new InvalidOperationException(
                "El motivo es obligatorio (10 a 200 caracteres).");

        var pagos = await _pagoRepository
            .ObtenerPorPrestamoAsync(id);

        if (pagos.Any())
            throw new InvalidOperationException(
                "No se puede anular un préstamo que ya tiene pagos.");

        // Neutralizar períodos: quedan en "Anulado" sin pendiente
        // para no contaminar mora ni acumulados.
        var periodos = await _periodoInteresRepository
            .ObtenerPorPrestamoAsync(id);

        foreach (var periodo in periodos)
        {
            periodo.InteresPendiente = 0;
            periodo.Estado = "Anulado";
        }

        prestamo.Estado = "Anulado";

        await _prestamoRepository.ActualizarAsync(prestamo);
        await _prestamoRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task CrearPeriodosPendientesAsync(
        Guid prestamoId,
        DateTime fechaReferencia)
    {
        var prestamo =
            await _prestamoRepository.ObtenerPorIdAsync(prestamoId);

        if (prestamo is null)
            throw new InvalidOperationException(
                "El préstamo no existe.");

        if (prestamo.Estado != "Activo" &&
            prestamo.Estado != "Moroso")
            throw new InvalidOperationException(
                "Solo se pueden generar períodos para préstamos " +
                "activos o morosos.");

        // Diaria: cuotas fijas pre-generadas; solo el retorno
        // semanal post-plazo genera tramos nuevos.
        if (PeriodoInteresService.EsPlazoFijo(prestamo))
        {
            var periodosDiarios =
                await _periodoInteresRepository
                    .ObtenerPorPrestamoAsync(prestamoId);

            if (!periodosDiarios.Any() && prestamo.PlazoCuotas.HasValue)
                await CrearCuotasPlazoAsync(prestamo);

            await _periodoInteresRepository.GuardarCambiosAsync();

            if (_periodoInteresService is not null)
                await _periodoInteresService
                    .GenerarSemanalesPostPlazoAsync(
                        prestamoId, fechaReferencia);

            return;
        }

        var periodos =
            await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamoId);

        if (!periodos.Any())
        {
            await CrearSiguientePeriodoAsync(
                prestamo,
                prestamo.FechaInicio);

            await _periodoInteresRepository.GuardarCambiosAsync();

            return;
        }

        var ultimoPeriodo = periodos
            .OrderByDescending(x => x.FechaInicio)
            .First();

        var siguienteInicio = ultimoPeriodo.FechaVencimiento;

        while (siguienteInicio <= fechaReferencia)
        {
            await CrearSiguientePeriodoAsync(
                prestamo,
                siguienteInicio);

            siguienteInicio = siguienteInicio.AddDays(7);
        }

        await _periodoInteresRepository.GuardarCambiosAsync();
    }

    private async Task CrearSiguientePeriodoAsync(
        Prestamo prestamo,
        DateTime fechaInicio)
    {
        var periodos =
            await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamo.Id);

        var existe = periodos.Any(x =>
            x.FechaInicio == fechaInicio);

        if (existe)
            return;

        var fechaVencimiento = fechaInicio.AddDays(7);

        var interesGenerado = CalcularInteres(prestamo);

        var periodo = new PeriodoInteres
        {
            Id = Guid.NewGuid(),
            PrestamoId = prestamo.Id,
            FechaInicio = fechaInicio,
            FechaVencimiento = fechaVencimiento,
            InteresGenerado = interesGenerado,
            InteresPagado = 0,
            InteresPendiente = interesGenerado,
            Estado = "Pendiente",
            FechaPagoCompleto = null,
            FechaRegistro = DateTime.UtcNow
        };

        await _periodoInteresRepository.CrearAsync(periodo);
    }

    /// <summary>
    /// Genera las N cuotas del plazo fijo al aprobar (diarias
    /// Lun-Sáb sin domingos, o semanales) y fija FechaFin,
    /// MontoTotal y CuotaFija.
    /// </summary>
    private async Task CrearCuotasPlazoAsync(Prestamo prestamo)
    {
        if (!prestamo.PlazoCuotas.HasValue ||
            prestamo.PlazoCuotas.Value <= 0)
            throw new InvalidOperationException(
                "El plazo fijo requiere el plazo en cuotas.");

        var n = prestamo.PlazoCuotas.Value;
        var diaria = PeriodoInteresService.EsFrecuenciaDiaria(prestamo);

        // Semanas de interés: N días de cobro o N semanas.
        var semanas = diaria ? n / 7m : n;

        var interesTotal = Math.Round(
            prestamo.CapitalInicial *
                prestamo.TasaInteresSemanal *
                semanas,
            2,
            MidpointRounding.AwayFromZero);

        var montoTotal = prestamo.CapitalInicial + interesTotal;

        var cuota = Math.Ceiling(montoTotal / n * 10m) / 10m;

        var cuotas = PeriodoInteresService.ArmarCuotasPlazo(
            prestamo.FechaInicio, n, interesTotal, diaria);

        foreach (var (inicio, vencimiento, interes) in cuotas)
        {
            await _periodoInteresRepository.CrearAsync(
                new PeriodoInteres
                {
                    Id = Guid.NewGuid(),
                    PrestamoId = prestamo.Id,
                    FechaInicio = inicio,
                    FechaVencimiento = vencimiento,
                    InteresGenerado = interes,
                    InteresPagado = 0,
                    InteresPendiente = interes,
                    Estado = "Pendiente",
                    FechaPagoCompleto = null,
                    EsCuotaFija = true,
                    FechaRegistro = DateTime.UtcNow
                });
        }

        prestamo.FechaFin = cuotas.Last().Vencimiento;
        prestamo.MontoTotal = montoTotal;
        prestamo.CuotaFija = cuota;

        await _prestamoRepository.ActualizarAsync(prestamo);
    }

    private static decimal CalcularInteres(Prestamo prestamo)
    {
        // Fijo: siempre sobre el capital inicial. Saldo: sobre
        // el pendiente al generarse la semana (snapshot, no se
        // reescribe con pagos posteriores). RN-PRE-003: HALF_UP.
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

    private static PrestamoDto MapearDto(Prestamo prestamo)
    {
        return new PrestamoDto
        {
            Id = prestamo.Id,
            ClienteId = prestamo.ClienteId,
            ClienteNombre = prestamo.Cliente is null
                ? string.Empty
                : $"{prestamo.Cliente.Nombres} {prestamo.Cliente.Apellidos}",
            GaranteId = prestamo.GaranteId,
            CapitalInicial = prestamo.CapitalInicial,
            TasaInteresSemanal = prestamo.TasaInteresSemanal,
            EsquemaInteres = prestamo.EsquemaInteres,
            ModalidadCobro = prestamo.ModalidadCobro,
            FrecuenciaCuota = prestamo.FrecuenciaCuota,
            PlazoCuotas = prestamo.PlazoCuotas,
            FechaFin = prestamo.FechaFin,
            MontoTotal = prestamo.MontoTotal,
            CuotaFija = prestamo.CuotaFija,
            CapitalPendiente = prestamo.CapitalPendiente,
            FechaInicio = prestamo.FechaInicio,
            FechaAprobacion = prestamo.FechaAprobacion,
            Estado = prestamo.Estado
        };
    }
}