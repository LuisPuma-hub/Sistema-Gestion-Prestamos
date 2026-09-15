using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorIdAsync(Guid id);

    Task<Usuario?> ObtenerPorEmailAsync(string email);

    Task<IEnumerable<Usuario>> ObtenerTodosAsync();

    Task<Usuario> CrearAsync(Usuario usuario);

    Task ActualizarAsync(Usuario usuario);

    Task GuardarCambiosAsync();
}