namespace SistemaPrestamos.Domain.Entities;

public class Prestamo
{
    public Guid Id { get; set; }

    public Guid ClienteId { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public Guid? GaranteId { get; set; }

    public Garante? Garante { get; set; }

    public decimal CapitalInicial { get; set; }

    public decimal TasaInteresSemanal { get; set; } = 0.05m;

    public string EsquemaInteres { get; set; } = "Saldo";

    // Modalidad de cobro: Semanal (sin fecha fin, interés
    // semanal Fijo/Saldo) o PlazoFijo (con fecha inicio y fin,
    // frecuencia Diaria o Semanal; vencido el plazo vuelve al
    // 5% semanal sobre el saldo).
    public string ModalidadCobro { get; set; } = "Semanal";

    // Diaria o Semanal (solo PlazoFijo).
    public string FrecuenciaCuota { get; set; } = "Diaria";

    // N de cuotas del plazo (días de cobro Lun-Sáb en Diaria,
    // semanas en Semanal).
    public int? PlazoCuotas { get; set; }

    // Vencimiento de la última cuota del plazo.
    public DateTime? FechaFin { get; set; }

    // Total pactado (capital + interés prorrateado).
    public decimal MontoTotal { get; set; }

    // Cuota del plazo redondeada hacia arriba al décimo.
    public decimal CuotaFija { get; set; }

    public decimal CapitalPendiente { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<PeriodoInteres> PeriodosInteres { get; set; } = new List<PeriodoInteres>();
}