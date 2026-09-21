namespace SistemaPrestamos.Domain.Entities;

public class Auditoria
{
    public Guid Id { get; set; }

    public string Tabla { get; set; } = string.Empty;

    public string IdRegistro { get; set; } = string.Empty;

    public string Operacion { get; set; } = string.Empty;

    public string? Diff { get; set; }

    public Guid? UsuarioId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}
