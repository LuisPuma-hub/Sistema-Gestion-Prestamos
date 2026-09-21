namespace SistemaPrestamos.Domain.Entities;

public class Dispositivo
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public string Token { get; set; } = string.Empty;

    public string Plataforma { get; set; } = "android";

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
}
