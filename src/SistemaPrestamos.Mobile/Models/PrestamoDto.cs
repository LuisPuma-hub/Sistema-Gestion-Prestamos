namespace SistemaPrestamos.Mobile.Models;

public class PrestamoDto
{
    public Guid Id { get; set; }

    public Guid ClienteId { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;

    public Guid? GaranteId { get; set; }

    public decimal CapitalInicial { get; set; }

    public decimal TasaInteresSemanal { get; set; }

    public string EsquemaInteres { get; set; } = "Fijo";

    public decimal CapitalPendiente { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string Estado { get; set; } = string.Empty;

    public decimal InteresSemanal => CapitalInicial * TasaInteresSemanal;

    public bool EsSaldo => string.Equals(
        EsquemaInteres, "Saldo", StringComparison.OrdinalIgnoreCase);

    public string TextoBase => EsSaldo ? "s/saldo" : "s/inicial";

    public string TextoInteres => EsSaldo
        ? $"Interés semanal 5% s/saldo (actual " +
          $"S/ {CapitalPendiente * TasaInteresSemanal:N2}/sem.)"
        : $"Interés semanal 5% s/inicial: " +
          $"S/ {InteresSemanal:N2}/sem.";

    public string Etiqueta =>
        $"{ClienteNombre} — S/ {CapitalInicial:N2} " +
        $"({FechaInicio:dd/MM/yyyy})";
}
