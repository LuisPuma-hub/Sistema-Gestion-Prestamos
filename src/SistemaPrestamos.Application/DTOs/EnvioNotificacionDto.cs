namespace SistemaPrestamos.Application.DTOs;

public class EnvioNotificacionDto
{
    public Guid Id { get; set; }

    public string Evento { get; set; } = string.Empty;

    public string Canal { get; set; } = string.Empty;

    public string Destinatario { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string? Detalle { get; set; }

    public DateTime FechaCreacion { get; set; }
}
