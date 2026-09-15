namespace SistemaPrestamos.Domain.Entities;

public class PeriodoInteres
{
    public Guid Id { get; set; }

    public Guid PrestamoId { get; set; }

    public Prestamo Prestamo { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public decimal InteresGenerado { get; set; }

    public decimal InteresPagado { get; set; }

    public decimal InteresPendiente { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public DateTime? FechaPagoCompleto { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}