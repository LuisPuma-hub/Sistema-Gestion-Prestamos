namespace SistemaPrestamos.Mobile.Models;

public class PagoDto
{
    public Guid Id { get; set; }

    public Guid PrestamoId { get; set; }

    public decimal Monto { get; set; }

    public decimal MontoInteres { get; set; }

    public decimal MontoCapital { get; set; }

    public decimal CapitalPendiente { get; set; }

    public DateTime FechaPago { get; set; }

    public string? Comprobante { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string PrestamoNombre { get; set; } = string.Empty;
}
