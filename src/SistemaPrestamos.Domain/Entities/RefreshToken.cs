namespace SistemaPrestamos.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiraEn { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaRevocacion { get; set; }

    public bool Revocado => FechaRevocacion.HasValue;

    public bool Expirado => DateTime.UtcNow >= ExpiraEn;

    public bool Vigente => !Revocado && !Expirado;
}
