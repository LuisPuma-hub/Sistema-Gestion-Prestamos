using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IPrestamoService
{
    Task<IEnumerable<PrestamoDto>> ObtenerTodosAsync();

    Task<PrestamoDto?> ObtenerPorIdAsync(Guid id);

    Task<IEnumerable<PrestamoDto>> ObtenerPorClienteAsync(
        Guid clienteId);

    Task<PrestamoDto> CrearAsync(
        CrearPrestamoDto dto);

    Task<bool> AprobarAsync(Guid id);

    Task CrearPeriodosPendientesAsync(
        Guid prestamoId,
        DateTime fechaReferencia);
}