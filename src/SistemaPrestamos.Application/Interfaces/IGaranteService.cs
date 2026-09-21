using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IGaranteService
{
    Task<IEnumerable<GaranteDto>> ObtenerPorClienteAsync(Guid clienteId);

    Task<GaranteDto?> ObtenerPorIdAsync(Guid id);

    Task<GaranteDto> CrearAsync(CrearGaranteDto dto);

    Task<bool> ActualizarAsync(Guid id, CrearGaranteDto dto);

    Task<bool> EliminarAsync(Guid id);
}
