namespace SistemaPrestamos.Domain.Entities;

/// <summary>
/// Compromiso de pago de un cliente ("paga el lunes").
/// Lo crea el administrador; se cumple solo con un pago
/// registrado ese día; si no, queda vencido.
/// </summary>
public class NotaCompromiso
{
    public Guid Id { get; set; }

    public Guid ClienteId { get; set; }

    public Cliente Cliente { get; set; } = null!;

    // Préstamo específico (opcional). Si es null, cualquier
    // pago del cliente ese día lo cumple.
    public Guid? PrestamoId { get; set; }

    public Prestamo? Prestamo { get; set; }

    public string Texto { get; set; } = string.Empty;

    public DateTime FechaCompromiso { get; set; }

    // Pendiente | Cumplida | Vencida | Anulada
    public string Estado { get; set; } = "Pendiente";

    public Guid? CreadoPor { get; set; }

    public DateTime? FechaCumplimiento { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
