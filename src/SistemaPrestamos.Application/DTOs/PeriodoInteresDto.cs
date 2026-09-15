namespace SistemaPrestamos.Application.DTOs;

public class PeriodoInteresDto
{
    public Guid Id { get; set; }
    public Guid PrestamoId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal InteresGenerado { get; set; }
    public decimal InteresPagado { get; set; }
    public decimal InteresPendiente { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime? FechaPagoCompleto { get; set; }
    public DateTime FechaRegistro { get; set; }
}