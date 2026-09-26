namespace SistemaPrestamos.Application.DTOs;

public class AuditoriaDto
{
    public Guid Id { get; set; }

    public string Tabla { get; set; } = string.Empty;

    public string IdRegistro { get; set; } = string.Empty;

    public string Operacion { get; set; } = string.Empty;

    public string Diff { get; set; } = string.Empty;

    public Guid? UsuarioId { get; set; }

    public DateTime Fecha { get; set; }
}
