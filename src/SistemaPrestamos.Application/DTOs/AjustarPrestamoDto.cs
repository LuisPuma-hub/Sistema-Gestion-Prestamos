namespace SistemaPrestamos.Application.DTOs;

public class AjustarPrestamoDto
{
    public decimal? NuevoCapital { get; set; }

    public bool PerdonarIntereses { get; set; }

    public string Motivo { get; set; } = string.Empty;
}
