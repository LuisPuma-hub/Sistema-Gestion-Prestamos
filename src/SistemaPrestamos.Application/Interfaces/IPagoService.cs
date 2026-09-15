using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IPagoService
{
    Task<IEnumerable<PagoDto>> ObtenerTodosAsync();
    Task<PagoDto?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<PagoDto>> ObtenerPorPrestamoAsync(Guid prestamoId);
    Task<PagoDto> RegistrarAsync(CrearPagoDto dto);
}