using Microsoft.Extensions.Logging;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class ProgramadorService : IProgramadorService
{
    private readonly ILogger<ProgramadorService>? _logger;
    private readonly IReglaNotificacionRepository _reglaRepository;
    private readonly IEnvioNotificacionRepository _envioRepository;
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IPeriodoInteresRepository _periodoRepository;
    private readonly IMorosidadRepository _morosidadRepository;
    private readonly IPagoRepository _pagoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IWhatsappService _whatsappService;
    private readonly INotificacionService _notificacionService;

    public ProgramadorService(
        IReglaNotificacionRepository reglaRepository,
        IEnvioNotificacionRepository envioRepository,
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoRepository,
        IMorosidadRepository morosidadRepository,
        IPagoRepository pagoRepository,
        IUsuarioRepository usuarioRepository,
        IWhatsappService whatsappService,
        INotificacionService notificacionService,
        ILogger<ProgramadorService>? logger = null)
    {
        _logger = logger;
        _reglaRepository = reglaRepository;
        _envioRepository = envioRepository;
        _prestamoRepository = prestamoRepository;
        _periodoRepository = periodoRepository;
        _morosidadRepository = morosidadRepository;
        _pagoRepository = pagoRepository;
        _usuarioRepository = usuarioRepository;
        _whatsappService = whatsappService;
        _notificacionService = notificacionService;
    }

    public static DateTime AhoraLima(DateTime ahoraUtc)
    {
        TimeZoneInfo zona;

        try
        {
            zona = TimeZoneInfo.FindSystemTimeZoneById(
                "SA Pacific Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            zona = TimeZoneInfo.FindSystemTimeZoneById("America/Lima");
        }

        return TimeZoneInfo.ConvertTimeFromUtc(ahoraUtc, zona);
    }

    public static bool TocaHoy(ReglaNotificacion regla, DateTime ahoraLima)
    {
        var bit = 1 << ((int)(ahoraLima.DayOfWeek + 6) % 7);

        return (regla.DiasSemana & bit) != 0;
    }

    /// <summary>
    /// Minutos de gracia: si el tick no cayó en el minuto exacto
    /// (reinicio, regla creada tarde), igual ejecuta dentro de la ventana.
    /// </summary>
    public const int VentanaToleranciaMinutos = 15;

    public static bool TocaAhora(ReglaNotificacion regla, DateTime ahoraLima)
    {
        var horaHoy = ahoraLima.Date.Add(regla.Hora.ToTimeSpan());
        var atraso = ahoraLima - horaHoy;

        return atraso >= TimeSpan.Zero &&
            atraso.TotalMinutes <= VentanaToleranciaMinutos;
    }

    public async Task<int> EjecutarPendientesAsync(DateTime ahoraUtc)
    {
        var ahora = AhoraLima(ahoraUtc);
        var hoy = ahora.Date;

        var reglas = await _reglaRepository.ObtenerTodasAsync();

        var total = 0;

        foreach (var regla in reglas.Where(r =>
                     r.Activa &&
                     TocaHoy(r, ahora) &&
                     TocaAhora(r, ahora) &&
                     (r.UltimaEjecucion is null ||
                      AhoraLima(r.UltimaEjecucion.Value).Date < hoy)))
        {
            total += await EjecutarReglaAsync(regla, hoy);

            regla.UltimaEjecucion = ahoraUtc;

            await _reglaRepository.ActualizarAsync(regla);
            await _reglaRepository.GuardarCambiosAsync();
        }

        return total;
    }

    private async Task<int> EjecutarReglaAsync(
        ReglaNotificacion regla,
        DateTime hoy)
    {
        return regla.Evento switch
        {
            EventosNotificacion.VenceHoy =>
                await EnviarVencimientosAsync(regla, hoy, hoy),
            EventosNotificacion.VenceManana =>
                await EnviarVencimientosAsync(regla, hoy.AddDays(1), hoy),
            EventosNotificacion.MoraNueva =>
                await EnviarMorasAsync(regla, hoy, soloNuevas: true),
            EventosNotificacion.MoraPersistente =>
                await EnviarMorasAsync(regla, hoy, soloNuevas: false),
            EventosNotificacion.ResumenDiario =>
                await EnviarResumenAsync(regla, hoy),
            EventosNotificacion.MoraCobrador =>
                await EnviarMoraCobradorAsync(regla, hoy),
            EventosNotificacion.CobradoDia =>
                await EnviarCobradoDiaAsync(regla, hoy),
            EventosNotificacion.PrestamoPorAprobar =>
                await EnviarPrestamosPorAprobarAsync(regla, hoy),
            EventosNotificacion.ResumenVencimientos =>
                await EnviarResumenVencimientosAsync(regla, hoy),
            _ => 0
        };
    }

    private async Task<int> EnviarVencimientosAsync(
        ReglaNotificacion regla,
        DateTime fechaVencimiento,
        DateTime hoy)
    {
        var periodos = await _periodoRepository
            .ObtenerConVencimientoAsync(fechaVencimiento);

        _logger?.LogInformation(
            "Regla {Regla}: fecha {Fecha:yyyy-MM-dd}, periodos {Total}.",
            regla.Nombre,
            fechaVencimiento.Date,
            periodos.Count());

        var enviados = 0;

        foreach (var grupo in periodos.GroupBy(p => p.PrestamoId))
        {            var prestamo = grupo.First().Prestamo;

            if (await YaEnviadoAsync(regla, hoy, prestamo.ClienteId, prestamo.Id, null))
            {
                continue;
            }

            try
            {
                await _whatsappService.EnviarRecordatorioAsync(
                    prestamo.ClienteId,
                    prestamo.Id);

                await RegistrarAsync(
                    regla, prestamo.ClienteId, prestamo.Id, null,
                    $"{prestamo.Cliente.Nombres} ({prestamo.Cliente.Telefono})",
                    "Enviado", null, hoy);

                enviados++;
            }
            catch (Exception ex)
            {
                await RegistrarAsync(
                    regla, prestamo.ClienteId, prestamo.Id, null,
                    prestamo.Cliente.Nombres, "Fallido", ex.Message, hoy);
            }
        }

        return enviados;
    }

    private async Task<int> EnviarMorasAsync(
        ReglaNotificacion regla,
        DateTime hoy,
        bool soloNuevas)
    {
        var morosidades = await _morosidadRepository.ObtenerActivasAsync();

        var enviados = 0;

        foreach (var mora in morosidades)
        {
            var esNueva = mora.FechaInicio?.Date >= hoy;

            if (soloNuevas != esNueva)
            {
                continue;
            }

            var prestamo = mora.Prestamo;

            if (await YaEnviadoAsync(regla, hoy, prestamo.ClienteId, prestamo.Id, null))
            {
                continue;
            }

            try
            {
                await _whatsappService.EnviarPlantillaCatalogoAsync(
                    prestamo.ClienteId,
                    prestamo.Id,
                    regla.Plantilla ?? "aviso_mora");

                await RegistrarAsync(
                    regla, prestamo.ClienteId, prestamo.Id, null,
                    $"{prestamo.Cliente.Nombres} ({prestamo.Cliente.Telefono})",
                    "Enviado", null, hoy);

                enviados++;
            }
            catch (Exception ex)
            {
                await RegistrarAsync(
                    regla, prestamo.ClienteId, prestamo.Id, null,
                    prestamo.Cliente.Nombres, "Fallido", ex.Message, hoy);
            }
        }

        return enviados;
    }

    private async Task<List<Usuario>> ObtenerCobradoresAsync()
    {
        var usuarios = await _usuarioRepository.ObtenerTodosAsync();

        return usuarios
            .Where(u =>
                u.Activo &&
                (u.Rol == "Administrador" || u.Rol == "Cobrador"))
            .ToList();
    }

    private async Task<int> EnviarPushCobradoresAsync(
        ReglaNotificacion regla,
        DateTime hoy,
        string titulo,
        string cuerpo)
    {
        var cobradores = await ObtenerCobradoresAsync();
        var enviados = 0;

        foreach (var usuario in cobradores)
        {
            if (await YaEnviadoAsync(regla, hoy, null, null, usuario.Id))
            {
                continue;
            }

            try
            {
                var n = await _notificacionService.EnviarAUsuarioAsync(
                    usuario.Id, titulo, cuerpo);

                await RegistrarAsync(
                    regla, null, null, usuario.Id, usuario.Email,
                    n > 0 ? "Enviado" : "Fallido",
                    n > 0 ? null : "Sin dispositivos.", hoy);

                if (n > 0)
                {
                    enviados++;
                }
            }
            catch (Exception ex)
            {
                await RegistrarAsync(
                    regla, null, null, usuario.Id, usuario.Email,
                    "Fallido", ex.Message, hoy);
            }
        }

        return enviados;
    }

    private async Task<int> EnviarResumenAsync(
        ReglaNotificacion regla,
        DateTime hoy)
    {
        var vencenHoy = (await _periodoRepository
            .ObtenerConVencimientoAsync(hoy))
            .Select(p => p.PrestamoId)
            .Distinct()
            .Count();

        var enMora = (await _morosidadRepository.ObtenerActivasAsync())
            .Count();

        return await EnviarPushCobradoresAsync(
            regla,
            hoy,
            "Resumen de cobranza",
            $"Hoy vencen {vencenHoy} y hay {enMora} en mora.");
    }

    private async Task<int> EnviarMoraCobradorAsync(
        ReglaNotificacion regla,
        DateTime hoy)
    {
        var nuevas = (await _morosidadRepository.ObtenerActivasAsync())
            .Where(m => m.FechaInicio?.Date >= hoy)
            .ToList();

        if (nuevas.Count == 0)
        {
            return 0;
        }

        var nombres = string.Join(
            ", ",
            nuevas
                .Take(3)
                .Select(m => m.Prestamo.Cliente.Nombres));

        var extra = nuevas.Count > 3
            ? $" y {nuevas.Count - 3} más"
            : string.Empty;

        return await EnviarPushCobradoresAsync(
            regla,
            hoy,
            "Nueva mora",
            $"{nuevas.Count} entraron en mora hoy: {nombres}{extra}.");
    }

    private async Task<int> EnviarCobradoDiaAsync(
        ReglaNotificacion regla,
        DateTime hoy)
    {
        var pagos = await _pagoRepository.ObtenerPorFechaAsync(hoy);

        var lista = pagos.ToList();

        if (lista.Count == 0)
        {
            return 0;
        }

        var total = lista.Sum(p => p.Monto);

        return await EnviarPushCobradoresAsync(
            regla,
            hoy,
            "Cobrado del día",
            $"Hoy se cobró S/ {total:N2} en {lista.Count} pago(s).");
    }

    private async Task<int> EnviarPrestamosPorAprobarAsync(
        ReglaNotificacion regla,
        DateTime hoy)
    {
        var pendientes = (await _prestamoRepository.ObtenerTodosAsync())
            .Where(p => p.Estado == "Pendiente")
            .ToList();

        if (pendientes.Count == 0)
        {
            return 0;
        }

        return await EnviarPushCobradoresAsync(
            regla,
            hoy,
            "Préstamos por aprobar",
            $"Hay {pendientes.Count} préstamo(s) pendientes de aprobación.");
    }

    private async Task<int> EnviarResumenVencimientosAsync(
        ReglaNotificacion regla,
        DateTime hoy)
    {
        var vencenHoy = (await _periodoRepository
            .ObtenerConVencimientoAsync(hoy))
            .Select(p => p.PrestamoId)
            .Distinct()
            .Count();

        if (vencenHoy == 0)
        {
            return 0;
        }

        return await EnviarPushCobradoresAsync(
            regla,
            hoy,
            "Vencimientos de hoy",
            $"Hoy vencen {vencenHoy} préstamo(s).");
    }

    private async Task<bool> YaEnviadoAsync(
        ReglaNotificacion regla,
        DateTime hoy,
        Guid? clienteId,
        Guid? prestamoId,
        Guid? usuarioId)
    {
        return await _envioRepository.ExisteHoyAsync(
            regla.Id,
            regla.Evento,
            regla.Canal,
            clienteId,
            prestamoId,
            usuarioId,
            hoy);
    }

    private async Task RegistrarAsync(
        ReglaNotificacion regla,
        Guid? clienteId,
        Guid? prestamoId,
        Guid? usuarioId,
        string destinatario,
        string estado,
        string? detalle,
        DateTime hoy)
    {
        await _envioRepository.CrearAsync(new EnvioNotificacion
        {
            Id = Guid.NewGuid(),
            ReglaId = regla.Id,
            Evento = regla.Evento,
            Canal = regla.Canal,
            ClienteId = clienteId,
            PrestamoId = prestamoId,
            UsuarioId = usuarioId,
            Destinatario = destinatario,
            Estado = estado,
            Detalle = detalle,
            FechaCreacion = DateTime.UtcNow
        });

        await _envioRepository.GuardarCambiosAsync();
    }
}
