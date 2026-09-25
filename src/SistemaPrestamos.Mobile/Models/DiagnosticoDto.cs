namespace SistemaPrestamos.Mobile.Models;

public class DiagnosticoDto
{
    public string HoraUtc { get; set; } = string.Empty;

    public string HoraLima { get; set; } = string.Empty;

    public string? UltimoTickUtc { get; set; }

    public int ReglasActivas { get; set; }

    public List<string> TocanAhora { get; set; } = new();
}
