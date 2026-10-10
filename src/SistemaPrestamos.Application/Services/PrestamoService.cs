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

    public PrestamoService(
        IPrestamoRepository prestamoRepository,
        IClienteRepository clienteRepository,
        IPeriodoInteresRepository periodoInteresRepository,
        IPagoRepository pagoRepository,
        IWhatsappService? whatsappService = null)
    {
        _prestamoRepository = prestamoRepository;
        _clienteRepository = clienteRepository;
        _periodoInteresRepository = periodoInteresRepository;
        _pagoRepository = pagoRepository;
        _whatsappService = whatsappService;
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
            ? "Fijo"
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
            esquema, "Saldo",
            StringComparison.OrdinalIgnoreCase)
                ? "Saldo"
                : "Fijo";

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
            CapitalPendiente = prestamo.CapitalPendiente,
            FechaInicio = prestamo.FechaInicio,
            FechaAprobacion = prestamo.FechaAprobacion,
            Estado = prestamo.Estado
        };
    }
}