namespace SistemaPrestamos.Domain.Entities;

/// <summary>
/// Registro de cada aviso enviado (auditoría + antispam).
/// </summary>
public class EnvioNotificacion
{
    public Guid Id { get; set; }

    public Guid? ReglaId { get; set; }

    public ReglaNotificacion? Regla { get; set; }

    public string Evento { get; set; } = string.Empty;

    public string Canal { get; set; } = string.Empty;

    public Guid? ClienteId { get; set; }

    public Guid? PrestamoId { get; set; }

    public Guid? UsuarioId { get; set; }

    public string Destinatario { get; set; } = string.Empty;

    public string Estado { get; set; } = "Enviado";

    public string? Detalle { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
