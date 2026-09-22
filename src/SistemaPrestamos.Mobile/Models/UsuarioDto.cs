namespace SistemaPrestamos.Mobile.Models;

public class UsuarioDto
{
    public Guid Id { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    public string Inicial => Nombres.Length > 0
        ? Nombres.Substring(0, 1).ToUpperInvariant()
        : "?";
}
