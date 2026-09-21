using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class PagoService : IPagoService
{
    private readonly IPagoRepository _pagoRepository;
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IPeriodoInteresRepository _periodoInteresRepository;
    private readonly IPeriodoInteresService _periodoInteresService;

    public PagoService(
        IPagoRepository pagoRepository,
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoInteresRepository,
        IPeriodoInteresService periodoInteresService)
    {
        _pagoRepository = pagoRepository;
        _prestamoRepository = prestamoRepository;
        _periodoInteresRepository = periodoInteresRepository;
        _periodoInteresService = periodoInteresService;
    }

    public async Task<IEnumerable<PagoDto>> ObtenerTodosAsync()
    {
        var pagos = await _pagoRepository.ObtenerTodosAsync();

        var resultado = new List<PagoDto>();

        foreach (var pago in pagos)
        {
            resultado.Add(await MapearDtoAsync(pago));
        }

        return resultado;
    }

    public async Task<PagoDto?> ObtenerPorIdAsync(Guid id)
    {
        var pago = await _pagoRepository.ObtenerPorIdAsync(id);

        if (pago is null)
            return null;

        return await MapearDtoAsync(pago);
    }

    public async Task<IEnumerable<PagoDto>> ObtenerPorPrestamoAsync(
        Guid prestamoId)
    {
        var prestamo = await _prestamoRepository.ObtenerPorIdAsync(
            prestamoId);

        if (prestamo is null)
        {
            throw new InvalidOperationException(
                "El préstamo no existe.");
        }

        var pagos = await _pagoRepository.ObtenerPorPrestamoAsync(
            prestamoId);

        var resultado = new List<PagoDto>();

        foreach (var pago in pagos)
        {
            resultado.Add(await MapearDtoAsync(pago));
        }

        return resultado;
    }

    public async Task<PagoDto> RegistrarAsync(CrearPagoDto dto)
    {
        // 1. Validar préstamo
        if (dto.PrestamoId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "El préstamo es obligatorio.");
        }

        // 2. Validar monto
        if (dto.Monto <= 0)
        {
            throw new InvalidOperationException(
                "El monto del pago debe ser mayor que cero.");
        }

        // 3. Obtener préstamo
        var prestamo = await _prestamoRepository.ObtenerPorIdAsync(
            dto.PrestamoId);

        if (prestamo is null)
        {
            throw new InvalidOperationException(
                "El préstamo no existe.");
        }

        // 4. Validar estado
        if (prestamo.Estado != "Activo")
        {
            throw new InvalidOperationException(
                "Solo se pueden registrar pagos para préstamos activos.");
        }

        // 5. Fecha del pago (siempre UTC: el móvil envía
        //    fecha sin zona y PostgreSQL timestamptz la rechaza)
        var fechaPago = dto.FechaPago == default
            ? DateTime.UtcNow
            : DateTime.SpecifyKind(dto.FechaPago, DateTimeKind.Utc);

        // 5b. Nunca futura (RN-PAG-002)
        if (fechaPago.Date > DateTime.UtcNow.Date)
        {
            throw new InvalidOperationException(
                "La fecha del pago no puede ser futura.");
        }

        // 6. Generar automáticamente los períodos
        //    que correspondan hasta la fecha del pago.
        await _periodoInteresService.GenerarPeriodosPendientesAsync(
            prestamo.Id,
            fechaPago);

        // 7. Obtener períodos actualizados
        var periodos = await _periodoInteresRepository
            .ObtenerPorPrestamoAsync(prestamo.Id);

        var periodosPendientes = periodos
            .Where(x => x.InteresPendiente > 0)
            .OrderBy(x => x.FechaVencimiento)
            .ThenBy(x => x.FechaInicio)
            .ToList();

        // 8. Calcular deuda de intereses
        var interesesPendientes = periodosPendientes.Sum(
            x => x.InteresPendiente);

        // 9. Calcular deuda total
        var deudaTotal = interesesPendientes +
                         prestamo.CapitalPendiente;

        // 10. Evitar pagos superiores a la deuda.
        if (dto.Monto > deudaTotal)
        {
            throw new InvalidOperationException(
                $"El monto del pago ({dto.Monto:F2}) " +
                $"supera la deuda disponible ({deudaTotal:F2}).");
        }

        decimal montoRestante = dto.Monto;
        decimal montoInteres = 0;
        decimal montoCapital = 0;

        // ============================================================
        // 11. APLICAR PRIMERO A INTERESES
        // ============================================================

        foreach (var periodo in periodosPendientes)
        {
            if (montoRestante <= 0)
                break;

            var pagoInteres = Math.Min(
                montoRestante,
                periodo.InteresPendiente);

            periodo.InteresPagado += pagoInteres;
            periodo.InteresPendiente -= pagoInteres;

            montoInteres += pagoInteres;
            montoRestante -= pagoInteres;

            // Período completamente pagado
            if (periodo.InteresPendiente <= 0)
            {
                periodo.InteresPendiente = 0;
                periodo.Estado = "Pagado";
                periodo.FechaPagoCompleto = fechaPago;
            }
            // Período parcialmente pagado
            else
            {
                periodo.Estado = "Parcial";
            }
        }

        // ============================================================
        // 12. EL EXCEDENTE SE APLICA AL CAPITAL
        // ============================================================

        if (montoRestante > 0)
        {
            if (prestamo.CapitalPendiente <= 0)
            {
                throw new InvalidOperationException(
                    "No existe capital pendiente para aplicar el excedente.");
            }

            montoCapital = Math.Min(
                montoRestante,
                prestamo.CapitalPendiente);

            prestamo.CapitalPendiente -= montoCapital;
            montoRestante -= montoCapital;
        }

        // ============================================================
        // 13. VALIDACIÓN FINAL
        // ============================================================

        if (montoRestante > 0)
        {
            throw new InvalidOperationException(
                "No se pudo distribuir completamente el monto del pago.");
        }

        // ============================================================
        // 13b. CIERRE AUTOMÁTICO (RN-PRE-010)
        // El préstamo pasa a CANCELADO solo si no queda
        // capital ni intereses pendientes.
        // ============================================================

        var interesesRestantes = periodosPendientes.Sum(
            x => x.InteresPendiente);

        if (prestamo.CapitalPendiente == 0 && interesesRestantes == 0)
        {
            prestamo.Estado = "Cancelado";
        }

        // ============================================================
        // 14. CREAR REGISTRO DEL PAGO
        // ============================================================

        var pago = new Pago
        {
            Id = Guid.NewGuid(),
            PrestamoId = prestamo.Id,
            Monto = dto.Monto,
            MontoInteres = montoInteres,
            MontoCapital = montoCapital,
            FechaPago = fechaPago,
            Comprobante = dto.Comprobante,
            Observaciones = dto.Observaciones,
            FechaRegistro = DateTime.UtcNow
        };

        // ============================================================
        // 15. GUARDAR CAMBIOS
        // ============================================================

        await _pagoRepository.CrearAsync(pago);
        await _prestamoRepository.ActualizarAsync(prestamo);

        await _pagoRepository.GuardarCambiosAsync();

        // ============================================================
        // 16. RECUPERAR PAGO GUARDADO
        // ============================================================

        var pagoGuardado = await _pagoRepository.ObtenerPorIdAsync(
            pago.Id);

        if (pagoGuardado is null)
        {
            throw new InvalidOperationException(
                "No se pudo recuperar el pago registrado.");
        }

        return await MapearDtoAsync(pagoGuardado);
    }

    private async Task<PagoDto> MapearDtoAsync(Pago pago)
    {
        var prestamo = await _prestamoRepository.ObtenerPorIdAsync(
            pago.PrestamoId);

        return new PagoDto
        {
            Id = pago.Id,
            PrestamoId = pago.PrestamoId,
            Monto = pago.Monto,
            MontoInteres = pago.MontoInteres,
            MontoCapital = pago.MontoCapital,
            CapitalPendiente = prestamo?.CapitalPendiente ?? 0,
            FechaPago = pago.FechaPago,
            Comprobante = pago.Comprobante,
            Observaciones = pago.Observaciones,
            FechaRegistro = pago.FechaRegistro
        };
    }
}