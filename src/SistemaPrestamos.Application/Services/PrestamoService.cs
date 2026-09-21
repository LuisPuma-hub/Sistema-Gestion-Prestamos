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

    public PrestamoService(
        IPrestamoRepository prestamoRepository,
        IClienteRepository clienteRepository,
        IPeriodoInteresRepository periodoInteresRepository)
    {
        _prestamoRepository = prestamoRepository;
        _clienteRepository = clienteRepository;
        _periodoInteresRepository = periodoInteresRepository;
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

        var cliente =
            await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);

        if (cliente is null)
            throw new InvalidOperationException(
                "El cliente no existe.");

        if (cliente.Estado != "Activo")
            throw new InvalidOperationException(
                "Solo se pueden registrar préstamos para clientes activos.");

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

        prestamo.Estado = "Activo";
        prestamo.FechaAprobacion = DateTime.UtcNow;

        await _prestamoRepository.ActualizarAsync(prestamo);

        // Crear el primer período semanal.
        await CrearSiguientePeriodoAsync(
            prestamo,
            prestamo.FechaInicio);

        await _periodoInteresRepository.GuardarCambiosAsync();

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

        if (prestamo.Estado != "Activo")
            throw new InvalidOperationException(
                "Solo se pueden generar períodos para préstamos activos.");

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

        // El interés siempre se calcula sobre el capital inicial.
        var interesGenerado =
            prestamo.CapitalInicial *
            prestamo.TasaInteresSemanal;

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
            CapitalPendiente = prestamo.CapitalPendiente,
            FechaInicio = prestamo.FechaInicio,
            FechaAprobacion = prestamo.FechaAprobacion,
            Estado = prestamo.Estado
        };
    }
}