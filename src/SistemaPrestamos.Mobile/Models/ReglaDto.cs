namespace SistemaPrestamos.Mobile.Models;

public class ReglaDto
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Evento { get; set; } = string.Empty;

    public string Canal { get; set; } = string.Empty;

    public string Hora { get; set; } = string.Empty;

    public int DiasSemana { get; set; }

    public string? Plantilla { get; set; }

    public bool Activa { get; set; }
}
