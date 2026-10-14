namespace SistemaPrestamos.Application.DTOs;

public class AjustarPrestamoDto
{
    public decimal? NuevoCapital { get; set; }

    public bool PerdonarIntereses { get; set; }

    // Perdón parcial: tope sobre intereses vencidos (los más
    // antiguos primero). Si viene, manda sobre PerdonarIntereses.
    public decimal? MontoPerdonar { get; set; }

    public string Motivo { get; set; } = string.Empty;
}
