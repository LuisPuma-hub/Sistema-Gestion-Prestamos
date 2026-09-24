namespace SistemaPrestamos.Domain.Entities;

public static class EventosNotificacion
{
    public const string VenceHoy = "VenceHoy";
    public const string VenceManana = "VenceManana";
    public const string MoraNueva = "MoraNueva";
    public const string MoraPersistente = "MoraPersistente";
    public const string ResumenCobrador = "ResumenCobrador";

    public static readonly IReadOnlyList<string> Todos =
    [
        VenceHoy,
        VenceManana,
        MoraNueva,
        MoraPersistente,
        ResumenCobrador
    ];
}

public static class CanalesNotificacion
{
    public const string Whatsapp = "Whatsapp";
    public const string Push = "Push";

    public static readonly IReadOnlyList<string> Todos =
    [
        Whatsapp,
        Push
    ];
}

/// <summary>
/// Regla programable: qué evento avisar, por qué canal,
/// a qué hora y qué días. El job la ejecuta una vez al día.
/// </summary>
public class ReglaNotificacion
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Evento { get; set; } = EventosNotificacion.VenceHoy;

    public string Canal { get; set; } = CanalesNotificacion.Whatsapp;

    public TimeOnly Hora { get; set; }

    /// <summary>
    /// Bitmask Lun=1 ... Dom=64. 127 = todos los días.
    /// </summary>
    public int DiasSemana { get; set; } = 127;

    public string? Plantilla { get; set; }

    public bool Activa { get; set; } = true;

    public DateTime? UltimaEjecucion { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
