namespace SistemaPrestamos.Domain.Entities;

public class Pago
{
    public Guid Id { get; set; }

    public Guid PrestamoId { get; set; }

    public Prestamo Prestamo { get; set; } = null!;

    public decimal Monto { get; set; }

    public decimal MontoInteres { get; set; }

    public decimal MontoCapital { get; set; }

    public DateTime FechaPago { get; set; }

    public string? Comprobante { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}