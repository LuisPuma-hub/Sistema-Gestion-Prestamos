namespace SistemaPrestamos.Mobile.Models;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public DateTime ExpiraEn { get; set; }
}
