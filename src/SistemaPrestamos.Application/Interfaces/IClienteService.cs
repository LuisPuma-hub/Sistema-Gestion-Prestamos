using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteDto>> ObtenerTodosAsync();

    Task<ClienteDto?> ObtenerPorIdAsync(Guid id);

    Task<ClienteDto?> ObtenerPorDocumentoAsync(string numeroDocumento);

    Task<ClienteDto> CrearAsync(CrearClienteDto dto);

    Task<bool> ActualizarAsync(Guid id, CrearClienteDto dto);

    Task<bool> CambiarEstadoAsync(Guid id, string estado);

    Task<bool> EliminarAsync(Guid id);

    Task<ResumenCascada> EliminarCascadaAsync(Guid id);

    Task<bool> ActualizarFotoAsync(Guid id, string ruta);
}