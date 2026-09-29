namespace SistemaPrestamos.Domain.Entities;

public class PasswordReset
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiraEn { get; set; }

    public bool Usado { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public bool Expirado => DateTime.UtcNow >= ExpiraEn;

    public bool Vigente => !Usado && !Expirado;
}
