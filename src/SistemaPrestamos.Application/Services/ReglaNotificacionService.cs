using System.Globalization;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class ReglaNotificacionService : IReglaNotificacionService
{
    private readonly IReglaNotificacionRepository _reglaRepository;
    private readonly IEnvioNotificacionRepository _envioRepository;
    private readonly IWhatsappService _whatsappService;
    private readonly INotificacionService _notificacionService;

    public ReglaNotificacionService(
        IReglaNotificacionRepository reglaRepository,
        IEnvioNotificacionRepository envioRepository,
        IWhatsappService whatsappService,
        INotificacionService notificacionService)
    {
        _reglaRepository = reglaRepository;
        _envioRepository = envioRepository;
        _whatsappService = whatsappService;
        _notificacionService = notificacionService;
    }

    public async Task<IEnumerable<ReglaNotificacionDto>> ListarAsync()
    {
        var reglas = await _reglaRepository.ObtenerTodasAsync();

        return reglas.Select(Mapear);
    }

    public async Task<ReglaNotificacionDto?> ObtenerAsync(Guid id)
    {
        var regla = await _reglaRepository.ObtenerPorIdAsync(id);

        return regla is null ? null : Mapear(regla);
    }

    public async Task<ReglaNotificacionDto> CrearAsync(CrearReglaDto dto)
    {
        Validar(dto.Nombre, dto.Evento, dto.Canal, dto.Hora, dto.DiasSemana, dto.Plantilla);

        var regla = new ReglaNotificacion
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre.Trim(),
            Evento = dto.Evento,
            Canal = dto.Canal,
            Hora = TimeOnly.ParseExact(dto.Hora, "HH:mm", CultureInfo.InvariantCulture),
            DiasSemana = dto.DiasSemana,
            Plantilla = string.IsNullOrWhiteSpace(dto.Plantilla)
                ? PlantillaPorDefecto(dto.Evento)
                : dto.Plantilla.Trim(),
            Activa = true,
            FechaCreacion = DateTime.UtcNow
        };

        await _reglaRepository.CrearAsync(regla);
        await _reglaRepository.GuardarCambiosAsync();

        return Mapear(regla);
    }

    public async Task<ReglaNotificacionDto?> ActualizarAsync(
        Guid id,
        ActualizarReglaDto dto)
    {
        var regla = await _reglaRepository.ObtenerPorIdAsync(id);

        if (regla is null)
        {
            return null;
        }

        Validar(dto.Nombre, dto.Evento, dto.Canal, dto.Hora, dto.DiasSemana, dto.Plantilla);

        regla.Nombre = dto.Nombre.Trim();
        regla.Evento = dto.Evento;
        regla.Canal = dto.Canal;
        regla.Hora = TimeOnly.ParseExact(dto.Hora, "HH:mm", CultureInfo.InvariantCulture);
        regla.DiasSemana = dto.DiasSemana;
        regla.Plantilla = string.IsNullOrWhiteSpace(dto.Plantilla)
            ? PlantillaPorDefecto(dto.Evento)
            : dto.Plantilla.Trim();
        regla.Activa = dto.Activa;
        regla.UltimaEjecucion = null;

        await _reglaRepository.ActualizarAsync(regla);
        await _reglaRepository.GuardarCambiosAsync();

        return Mapear(regla);
    }

    public async Task<ReglaNotificacionDto?> CambiarEstadoAsync(
        Guid id,
        bool activa)
    {
        var regla = await _reglaRepository.ObtenerPorIdAsync(id);

        if (regla is null)
        {
            return null;
        }

        regla.Activa = activa;

        await _reglaRepository.ActualizarAsync(regla);
        await _reglaRepository.GuardarCambiosAsync();

        return Mapear(regla);
    }

    public async Task<bool> EliminarAsync(Guid id)
    {
        var regla = await _reglaRepository.ObtenerPorIdAsync(id);

        if (regla is null)
        {
            return false;
        }

        await _reglaRepository.EliminarAsync(regla);
        await _reglaRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<(bool Exito, string Mensaje)> ProbarAsync(
        Guid id,
        Guid usuarioId,
        Guid? clienteId = null,
        Guid? prestamoId = null)
    {
        var regla = await _reglaRepository.ObtenerPorIdAsync(id);

        if (regla is null)
        {
            return (false, "La regla no existe.");
        }

        try
        {
            if (regla.Canal == CanalesNotificacion.Push)
            {
                var enviados = await _notificacionService.EnviarAUsuarioAsync(
                    usuarioId,
                    $"Prueba: {regla.Nombre}",
                    $"Regla '{regla.Evento}' verificada correctamente.");

                await RegistrarEnvioAsync(
                    regla, null, null, usuarioId, "Prueba",
                    enviados > 0 ? "Enviado" : "Fallido",
                    enviados > 0 ? null : "Sin dispositivos.");

                return enviados > 0
                    ? (true, $"Prueba enviada a {enviados} dispositivo(s).")
                    : (false, "No tienes dispositivos registrados.");
            }

            if (!clienteId.HasValue || !prestamoId.HasValue)
            {
                return (false, "La prueba de WhatsApp requiere cliente y préstamo.");
            }

            await _whatsappService.EnviarPlantillaCatalogoAsync(
                clienteId.Value,
                prestamoId,
                regla.Plantilla ?? PlantillaPorDefecto(regla.Evento));

            await RegistrarEnvioAsync(
                regla, clienteId, prestamoId, null, "Prueba", "Enviado", null);

            return (true, "Mensaje de prueba enviado por WhatsApp.");
        }
        catch (Exception ex)
        {
            await RegistrarEnvioAsync(
                regla, clienteId, prestamoId, null, "Prueba", "Fallido", ex.Message);

            return (false, $"La prueba falló: {ex.Message}");
        }
    }

    public async Task<IEnumerable<EnvioNotificacionDto>> ObtenerEnviosAsync(
        int top)
    {
        if (top is < 1 or > 500)
        {
            top = 50;
        }

        var envios = await _envioRepository.ObtenerRecientesAsync(top);

        return envios.Select(x => new EnvioNotificacionDto
        {
            Id = x.Id,
            Evento = x.Evento,
            Canal = x.Canal,
            Destinatario = x.Destinatario,
            Estado = x.Estado,
            Detalle = x.Detalle,
            FechaCreacion = x.FechaCreacion
        });
    }

    private static void Validar(
        string nombre,
        string evento,
        string canal,
        string hora,
        int dias,
        string? plantilla)
    {
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length > 120)
        {
            throw new InvalidOperationException(
                "El nombre es requerido (máx. 120).");
        }

        if (!EventosNotificacion.Todos.Contains(evento))
        {
            throw new InvalidOperationException(
                $"Evento no válido. Use: {string.Join(", ", EventosNotificacion.Todos)}.");
        }

        if (!CanalesNotificacion.Todos.Contains(canal))
        {
            throw new InvalidOperationException(
                "Canal no válido. Use: Whatsapp, Push.");
        }

        if (!TimeOnly.TryParseExact(
                hora, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            throw new InvalidOperationException(
                "Hora no válida. Use formato HH:mm (24h).");
        }

        if (dias is < 1 or > 127)
        {
            throw new InvalidOperationException(
                "Días de semana debe estar entre 1 y 127.");
        }

        if (canal == CanalesNotificacion.Push &&
            evento != EventosNotificacion.ResumenCobrador)
        {
            throw new InvalidOperationException(
                "El canal Push solo aplica al ResumenCobrador.");
        }

        if (canal == CanalesNotificacion.Whatsapp &&
            !string.IsNullOrWhiteSpace(plantilla) &&
            !WhatsappService.PlantillasDisponibles.Any(p =>
                string.Equals(p.Nombre, plantilla, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                "Plantilla de WhatsApp no disponible.");
        }
    }

    private static string? PlantillaPorDefecto(string evento)
    {
        return evento switch
        {
            EventosNotificacion.VenceHoy => "recordatorio_pago_v2",
            EventosNotificacion.VenceManana => "recordatorio_pago_v2",
            EventosNotificacion.MoraNueva => "aviso_mora",
            EventosNotificacion.MoraPersistente => "aviso_mora",
            _ => null
        };
    }

    private async Task RegistrarEnvioAsync(
        ReglaNotificacion regla,
        Guid? clienteId,
        Guid? prestamoId,
        Guid? usuarioId,
        string destinatario,
        string estado,
        string? detalle)
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

    private static ReglaNotificacionDto Mapear(ReglaNotificacion regla)
    {
        return new ReglaNotificacionDto
        {
            Id = regla.Id,
            Nombre = regla.Nombre,
            Evento = regla.Evento,
            Canal = regla.Canal,
            Hora = regla.Hora.ToString("HH:mm"),
            DiasSemana = regla.DiasSemana,
            Plantilla = regla.Plantilla,
            Activa = regla.Activa,
            UltimaEjecucion = regla.UltimaEjecucion
        };
    }
}
