namespace SistemaPrestamos.Domain.Entities;

public class FondoMovimiento
{
    public Guid Id { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public DateTime Fecha { get; set; }

    public string? Motivo { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
