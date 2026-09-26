using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IAuditoriaService
{
    Task<IEnumerable<AuditoriaDto>> ObtenerRecientesAsync(int top);
}
