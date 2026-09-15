namespace SistemaPrestamos.Domain.Entities;

public class Garante
{
    public Guid Id { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Direccion { get; set; }

    public Guid? ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}