namespace SistemaPrestamos.Application.DTOs;

public class CrearPagoDto
{
    public Guid PrestamoId { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public string? Comprobante { get; set; }
    public string? Observaciones { get; set; }
}