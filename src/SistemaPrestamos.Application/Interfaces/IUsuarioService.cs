using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();

    Task<UsuarioDto?> ObtenerPorIdAsync(Guid id);

    Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto);

    Task CambiarClaveAsync(Guid id, string actual, string nueva);

    Task ResetearClaveAsync(Guid id, string nueva);

    Task<bool> EliminarAsync(Guid id, Guid actorId);
}