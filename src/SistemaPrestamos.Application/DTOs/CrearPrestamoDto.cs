namespace SistemaPrestamos.Application.DTOs;

public class CrearPrestamoDto
{
    public Guid ClienteId { get; set; }

    public Guid? GaranteId { get; set; }

    public decimal CapitalInicial { get; set; }

    public DateTime FechaInicio { get; set; }

    public string EsquemaInteres { get; set; } = "Saldo";

    // Semanal (sin fecha fin) o PlazoFijo (con fecha fin).
    public string ModalidadCobro { get; set; } = "Semanal";

    // Diaria o Semanal (solo PlazoFijo).
    public string FrecuenciaCuota { get; set; } = "Diaria";

    // Cantidad del plazo (solo Diaria).
    public int? PlazoValor { get; set; }

    // Dias o Semanas (solo Diaria; Semanas x 7).
    public string PlazoUnidad { get; set; } = "Dias";
}