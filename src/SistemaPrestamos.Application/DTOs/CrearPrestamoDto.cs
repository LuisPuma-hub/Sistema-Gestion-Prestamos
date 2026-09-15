namespace SistemaPrestamos.Application.DTOs;

public class CrearPrestamoDto
{
    public Guid ClienteId { get; set; }

    public Guid? GaranteId { get; set; }

    public decimal CapitalInicial { get; set; }

    public DateTime FechaInicio { get; set; }
}