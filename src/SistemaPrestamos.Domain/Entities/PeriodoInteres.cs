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

    // True: cuota de plazo fijo (diaria Lun-Sáb o semanal).
    // False: tramo semanal sin fin (o semanal post-plazo).
    public bool EsCuotaFija { get; set; }

    public DateTime? FechaPagoCompleto { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}