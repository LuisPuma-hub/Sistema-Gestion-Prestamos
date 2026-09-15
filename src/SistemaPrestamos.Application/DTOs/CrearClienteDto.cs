namespace SistemaPrestamos.Application.DTOs;

public class CrearClienteDto
{
    public string TipoDocumento { get; set; } = string.Empty;

    public string NumeroDocumento { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? FotoReciboServicio { get; set; }

    public string? Observaciones { get; set; }
}