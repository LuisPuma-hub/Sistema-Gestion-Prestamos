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

    public decimal CapitalPendiente { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<PeriodoInteres> PeriodosInteres { get; set; } = new List<PeriodoInteres>();
}