using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();

    Task<UsuarioDto?> ObtenerPorIdAsync(Guid id);

    Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto);
}