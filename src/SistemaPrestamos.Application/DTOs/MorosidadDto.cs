namespace SistemaPrestamos.Application.DTOs;

public class MorosidadDto
{
    public Guid Id { get; set; }

    public Guid PrestamoId { get; set; }

    public int PagosInteresVencidos { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaReactivacion { get; set; }

    public bool Activa { get; set; }

    public string? Observaciones { get; set; }
}