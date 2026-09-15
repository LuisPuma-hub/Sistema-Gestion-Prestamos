namespace SistemaPrestamos.Domain.Entities;

public class Cliente
{
    public Guid Id { get; set; }

    public string TipoDocumento { get; set; } = string.Empty;

    public string NumeroDocumento { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? FotoReciboServicio { get; set; }

    public string? Observaciones { get; set; }

    public string Estado { get; set; } = "Activo";

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}