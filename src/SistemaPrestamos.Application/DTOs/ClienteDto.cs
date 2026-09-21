namespace SistemaPrestamos.Application.DTOs;

public class ClienteDto
{
    public Guid Id { get; set; }

    public string TipoDocumento { get; set; } = string.Empty;

    public string NumeroDocumento { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Direccion { get; set; }

    public string? ReferenciaDireccion { get; set; }

    public string? FotoReciboServicio { get; set; }

    public string? Observaciones { get; set; }

    public string Estado { get; set; } = string.Empty;

    public Guid? UsuarioRegistraId { get; set; }

    public DateTime FechaRegistro { get; set; }
}