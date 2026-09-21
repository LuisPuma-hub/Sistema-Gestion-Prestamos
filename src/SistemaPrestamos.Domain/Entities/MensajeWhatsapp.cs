namespace SistemaPrestamos.Domain.Entities;

public class MensajeWhatsapp
{
    public Guid Id { get; set; }

    public Guid? ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    public Guid? PrestamoId { get; set; }

    public Prestamo? Prestamo { get; set; }

    public string TipoPlantilla { get; set; } = string.Empty;

    public string NumeroDestino { get; set; } = string.Empty;

    public string Contenido { get; set; } = string.Empty;

    public string Estado { get; set; } = "Enviado";

    public string? IdentificadorExterno { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
