namespace SistemaPrestamos.Mobile.Models;

public class CompromisoDto
{
    public Guid Id { get; set; }

    public Guid ClienteId { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;

    public Guid? PrestamoId { get; set; }

    public string Texto { get; set; } = string.Empty;

    public DateTime FechaCompromiso { get; set; }

    public string Estado { get; set; } = string.Empty;

    public DateTime? FechaCumplimiento { get; set; }

    public string Linea =>
        $"{FechaCompromiso:dd/MM/yyyy} · {Texto} ({Estado})";
}
