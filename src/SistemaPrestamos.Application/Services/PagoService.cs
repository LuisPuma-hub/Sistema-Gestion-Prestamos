using System.Globalization;
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
    private readonly IWhatsappService? _whatsappService;
    private readonly IMorosidadRepository? _morosidadRepository;
    private readonly IMorosidadService? _morosidadService;

    public PagoService(
        IPagoRepository pagoRepository,
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoInteresRepository,
        IPeriodoInteresService periodoInteresService,
        IWhatsappService? whatsappService = null,
        IMorosidadRepository? morosidadRepository = null,
        IMorosidadService? morosidadService = null)
    {
        _pagoRepository = pagoRepository;
        _prestamoRepository = prestamoRepository;
        _periodoInteresRepository = periodoInteresRepository;
        _periodoInteresService = periodoInteresService;
        _whatsappService = whatsappService;
        _morosidadRepository = morosidadRepository;
        _morosidadService = morosidadService;
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

        // 4. Validar estado (un préstamo moroso también cobra:
        //    solo así puede regularizar; RN-PRE-012 lo reactiva).
        if (prestamo.Estado != "Activo" &&
            prestamo.Estado != "Moroso")
        {
            throw new InvalidOperationException(
                "Solo se pueden registrar pagos para préstamos " +
                "activos o morosos.");
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

        // 10b. RN-PAG-013: en mora el pago debe cubrir todos
        // los intereses vencidos acumulados (es lo que saca
        // de mora). Ej.: 3 semanas x S/ 15 = mínimo S/ 45.
        if (string.Equals(
                prestamo.Estado,
                "Moroso",
                StringComparison.OrdinalIgnoreCase))
        {
            var interesVencido = periodosPendientes
                .Where(x => x.FechaVencimiento < fechaPago)
                .Sum(x => x.InteresPendiente);

            if (dto.Monto < interesVencido)
                throw new InvalidOperationException(
                    "En mora el pago debe cubrir los intereses " +
                    $"vencidos (mínimo S/ {interesVencido:N2}).");
        }

        decimal montoRestante = dto.Monto;
        decimal montoInteres = 0;
        decimal montoCapital = 0;

        var aplicaciones = new List<(DateTime Venc, decimal Monto)>();

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

            if (pagoInteres > 0)
                aplicaciones.Add((periodo.FechaVencimiento, pagoInteres));

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

        // RN-PRE-012 (vía pago): si estaba en mora y ya no
        // cumple la condición (< 3 vencidos), vuelve solo a
        // ACTIVO y el cliente pasa a observación. Sin pago
        // que limpie, sigue en mora (solo sale por reactivar
        // manual con motivo).
        if (string.Equals(
                prestamo.Estado,
                "Moroso",
                StringComparison.OrdinalIgnoreCase))
        {
            var vencidosRestantes = periodosPendientes.Count(
                x => x.FechaVencimiento < fechaPago &&
                     x.InteresPendiente > 0);

            if (vencidosRestantes < 3)
            {
                prestamo.Estado = "Activo";

                if (_morosidadRepository is not null)
                {
                    var moraActiva = await _morosidadRepository
                        .ObtenerPorPrestamoAsync(prestamo.Id);

                    if (moraActiva is not null && moraActiva.Activa)
                    {
                        moraActiva.Activa = false;
                        moraActiva.FechaReactivacion = DateTime.UtcNow;

                        await _morosidadRepository
                            .ActualizarAsync(moraActiva);
                    }
                }

                if (prestamo.Cliente is not null)
                    await SincronizarClienteAsync(
                        prestamo.Cliente, prestamo.Id);
            }
        }

        if (prestamo.CapitalPendiente == 0 && interesesRestantes == 0)
        {
            prestamo.Estado = "Cancelado";
        }

        if (prestamo.Estado == "Cancelado")
        {
            if (_morosidadRepository is not null)
            {
                var mora = await _morosidadRepository
                    .ObtenerPorPrestamoAsync(prestamo.Id);

                if (mora is not null && mora.Activa)
                {
                    mora.Activa = false;
                    mora.FechaReactivacion = DateTime.UtcNow;

                    await _morosidadRepository
                        .ActualizarAsync(mora);
                }
            }

            // RN-MOR-009: sin préstamos en MOROSO el cliente
            // vuelve a ACTIVO y puede operar de nuevo (este
            // préstamo ya va a CANCELADO).
            if (prestamo.Cliente is not null)
                await SincronizarClienteAsync(
                    prestamo.Cliente, prestamo.Id);
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
            Estado = "Registrado",
            Detalle = ConstruirDetalle(aplicaciones, montoCapital),
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

        // Aviso automático al cliente (no bloquea el registro).
        if (_whatsappService is not null)
        {
            try
            {
                await _whatsappService.EnviarPlantillaCatalogoAsync(
                    pagoGuardado.Prestamo.ClienteId,
                    pagoGuardado.PrestamoId,
                    "confirmacion_pago");
            }
            catch
            {
            }
        }

        return await MapearDtoAsync(pagoGuardado);
    }

    private async Task SincronizarClienteAsync(
        Cliente cliente,
        Guid? ignorarPrestamoId = null)
    {
        var prestamos = await _prestamoRepository
            .ObtenerPorClienteAsync(cliente.Id);

        var hayMorosos = prestamos.Any(x =>
            (ignorarPrestamoId is null || x.Id != ignorarPrestamoId) &&
            string.Equals(
                x.Estado,
                "Moroso",
                StringComparison.OrdinalIgnoreCase));

        if (!hayMorosos &&
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

    public async Task<bool> AnularAsync(
        Guid pagoId,
        string motivo,
        Guid? anuladoPor)
    {
        if (string.IsNullOrWhiteSpace(motivo) ||
            motivo.Trim().Length < 10 ||
            motivo.Trim().Length > 200)
            throw new InvalidOperationException(
                "El motivo es obligatorio (10 a 200 caracteres).");

        var pago = await _pagoRepository.ObtenerPorIdAsync(pagoId);

        if (pago is null)
            return false;

        if (string.Equals(
                pago.Estado,
                "Anulado",
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "El pago ya está anulado.");

        var prestamo = await _prestamoRepository.ObtenerPorIdAsync(
            pago.PrestamoId);

        if (prestamo is null)
            throw new InvalidOperationException(
                "El préstamo no existe.");

        // Sin borrado físico (RN-PAG-011): se revierte la
        // distribución. El capital se devuelve primero (se
        // aplicó al último) y los intereses en LIFO.
        prestamo.CapitalPendiente += pago.MontoCapital;

        var restante = pago.MontoInteres;

        var periodos = (await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamo.Id))
            .Where(x => x.InteresPagado > 0)
            .OrderByDescending(x => x.FechaVencimiento)
            .ThenByDescending(x => x.FechaInicio)
            .ToList();

        foreach (var periodo in periodos)
        {
            if (restante <= 0)
                break;

            var quita = Math.Min(restante, periodo.InteresPagado);

            periodo.InteresPagado -= quita;
            periodo.InteresPendiente += quita;
            restante -= quita;

            if (periodo.InteresPendiente == periodo.InteresGenerado)
            {
                periodo.Estado = "Pendiente";
                periodo.FechaPagoCompleto = null;
            }
            else if (periodo.InteresPendiente <= 0)
            {
                periodo.InteresPendiente = 0;
                periodo.Estado = "Pagado";
            }
            else
            {
                periodo.Estado = "Parcial";
                periodo.FechaPagoCompleto = null;
            }
        }

        pago.Estado = "Anulado";
        pago.MotivoAnulacion = motivo.Trim();
        pago.FechaAnulacion = DateTime.UtcNow;
        pago.AnuladoPor = anuladoPor;

        if (string.Equals(
                prestamo.Estado,
                "Cancelado",
                StringComparison.OrdinalIgnoreCase))
            prestamo.Estado = "Activo";

        await _prestamoRepository.ActualizarAsync(prestamo);
        await _pagoRepository.GuardarCambiosAsync();

        // Reevaluar mora con el nuevo saldo (puede volver).
        if (_morosidadService is not null)
            await _morosidadService.EvaluarAsync(
                prestamo.Id, DateTime.UtcNow);

        return true;
    }

    private static string ConstruirDetalle(
        List<(DateTime Venc, decimal Monto)> aplicaciones,
        decimal montoCapital)
    {
        var partes = aplicaciones
            .Select((a, i) =>
                $"P{i + 1} {a.Venc:dd/MM}: " +
                $"S/ {a.Monto.ToString(
                    "N2", CultureInfo.InvariantCulture)} int.")
            .ToList();

        if (montoCapital > 0)
            partes.Add(
                $"Capital: S/ {montoCapital.ToString(
                    "N2", CultureInfo.InvariantCulture)}");

        return string.Join("; ", partes);
    }

    public async Task<PagoDto> AjustarAsync(
        Guid prestamoId,
        AjustarPrestamoDto dto,
        Guid? actor)
    {
        if (string.IsNullOrWhiteSpace(dto.Motivo) ||
            dto.Motivo.Trim().Length < 10 ||
            dto.Motivo.Trim().Length > 200)
            throw new InvalidOperationException(
                "El motivo es obligatorio (10 a 200 caracteres).");

        var prestamo = await _prestamoRepository
            .ObtenerPorIdAsync(prestamoId);

        if (prestamo is null)
            throw new InvalidOperationException(
                "El préstamo no existe.");

        if (!string.Equals(prestamo.Estado, "Activo",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(prestamo.Estado, "Moroso",
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "Solo se pueden ajustar préstamos activos o morosos.");

        var ahora = DateTime.UtcNow;
        var capitalAdj = 0m;
        var perdonado = 0m;
        var partes = new List<string>();

        if (dto.NuevoCapital.HasValue)
        {
            var nuevo = Math.Round(dto.NuevoCapital.Value, 2);

            if (nuevo < 0 || nuevo > prestamo.CapitalInicial)
                throw new InvalidOperationException(
                    "El nuevo capital debe estar entre 0 y el " +
                    "capital inicial.");

            var delta = prestamo.CapitalPendiente - nuevo;

            if (delta < 0)
                throw new InvalidOperationException(
                    "El ajuste solo puede reducir capital. Para " +
                    "aumentar, anule el pago o ajuste anterior.");

            if (delta > 0)
            {
                capitalAdj = delta;
                prestamo.CapitalPendiente = nuevo;

                partes.Add(
                    $"Capital {prestamo.CapitalPendiente + delta:N2} → " +
                    $"S/ {nuevo:N2}");
            }
        }

        if (dto.PerdonarIntereses)
        {
            var periodos = await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamo.Id);

            var vencidos = periodos
                .Where(x => x.InteresPendiente > 0 &&
                            x.FechaVencimiento < ahora)
                .OrderBy(x => x.FechaVencimiento)
                .ToList();

            foreach (var periodo in vencidos)
            {
                perdonado += periodo.InteresPendiente;
                periodo.InteresPagado = periodo.InteresGenerado;
                periodo.InteresPendiente = 0;
                periodo.Estado = "Pagado";
                periodo.FechaPagoCompleto = ahora;
            }

            if (vencidos.Count > 0)
                partes.Add(
                    $"CONDONACIÓN {vencidos.Count} sem.: " +
                    $"S/ {perdonado:N2}");
        }

        if (capitalAdj == 0 && perdonado == 0)
            throw new InvalidOperationException(
                "El ajuste no produce cambios.");

        var esCondonacion = perdonado > 0;

        var pago = new Pago
        {
            Id = Guid.NewGuid(),
            PrestamoId = prestamo.Id,
            Monto = capitalAdj + perdonado,
            MontoInteres = perdonado,
            MontoCapital = capitalAdj,
            FechaPago = ahora,
            Comprobante = null,
            Observaciones =
                (esCondonacion ? "CONDONACIÓN - " : "AJUSTE - ") +
                dto.Motivo.Trim(),
            Estado = "Ajuste",
            Detalle = string.Join("; ", partes),
            FechaRegistro = ahora
        };

        await _pagoRepository.CrearAsync(pago);
        await _prestamoRepository.ActualizarAsync(prestamo);

        var restantes = (await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamo.Id))
            .Sum(x => x.InteresPendiente);

        if (prestamo.CapitalPendiente == 0 && restantes == 0)
            prestamo.Estado = "Cancelado";
        else
            await ReconciliarMoraAsync(prestamo, ahora);

        await _pagoRepository.GuardarCambiosAsync();

        var guardado = await _pagoRepository.ObtenerPorIdAsync(
            pago.Id);

        if (guardado is null)
            throw new InvalidOperationException(
                "No se pudo recuperar el ajuste registrado.");

        return await MapearDtoAsync(guardado);
    }

    private async Task ReconciliarMoraAsync(
        Prestamo prestamo,
        DateTime referencia)
    {
        if (!string.Equals(
                prestamo.Estado,
                "Moroso",
                StringComparison.OrdinalIgnoreCase))
            return;

        var vencidos = (await _periodoInteresRepository
                .ObtenerPorPrestamoAsync(prestamo.Id))
            .Count(x => x.FechaVencimiento < referencia &&
                        x.InteresPendiente > 0);

        if (vencidos >= 3)
            return;

        prestamo.Estado = "Activo";

        if (_morosidadRepository is not null)
        {
            var mora = await _morosidadRepository
                .ObtenerPorPrestamoAsync(prestamo.Id);

            if (mora is not null && mora.Activa)
            {
                mora.Activa = false;
                mora.FechaReactivacion = referencia;

                await _morosidadRepository.ActualizarAsync(mora);
            }
        }

        if (prestamo.Cliente is not null)
            await SincronizarClienteAsync(
                prestamo.Cliente, prestamo.Id);
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
            Estado = pago.Estado,
            MotivoAnulacion = pago.MotivoAnulacion,
            Detalle = pago.Detalle,
            FechaRegistro = pago.FechaRegistro
        };
    }
}