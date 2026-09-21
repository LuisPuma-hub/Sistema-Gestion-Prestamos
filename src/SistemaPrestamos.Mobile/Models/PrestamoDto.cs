namespace SistemaPrestamos.Mobile.Models;

public class PrestamoDto
{
    public Guid Id { get; set; }

    public Guid ClienteId { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;

    public Guid? GaranteId { get; set; }

    public decimal CapitalInicial { get; set; }

    public decimal TasaInteresSemanal { get; set; }

    public decimal CapitalPendiente { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string Estado { get; set; } = string.Empty;

    public decimal InteresSemanal => CapitalInicial * TasaInteresSemanal;
}
