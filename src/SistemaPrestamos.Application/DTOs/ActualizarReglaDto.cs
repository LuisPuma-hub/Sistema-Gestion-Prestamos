namespace SistemaPrestamos.Application.DTOs;

public class ActualizarReglaDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Evento { get; set; } = string.Empty;

    public string Canal { get; set; } = string.Empty;

    public string Hora { get; set; } = string.Empty;

    public int DiasSemana { get; set; } = 127;

    public string? Plantilla { get; set; }

    public bool Activa { get; set; } = true;
}
