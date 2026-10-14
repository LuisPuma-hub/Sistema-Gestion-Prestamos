namespace SistemaPrestamos.Application.DTOs;

public class PrestamoDto
{
    public Guid Id { get; set; }

    public Guid ClienteId { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;

    public Guid? GaranteId { get; set; }

    public decimal CapitalInicial { get; set; }

    public decimal TasaInteresSemanal { get; set; }

    public string EsquemaInteres { get; set; } = "Saldo";

    public string ModalidadCobro { get; set; } = "Semanal";

    public string FrecuenciaCuota { get; set; } = "Diaria";

    public int? PlazoCuotas { get; set; }

    public DateTime? FechaFin { get; set; }

    public decimal MontoTotal { get; set; }

    public decimal CuotaFija { get; set; }

    public decimal CapitalPendiente { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string Estado { get; set; } = string.Empty;
}