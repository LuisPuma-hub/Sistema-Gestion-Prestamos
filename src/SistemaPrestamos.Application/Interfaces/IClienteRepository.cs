using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> ObtenerTodosAsync();

    Task<Cliente?> ObtenerPorIdAsync(Guid id);

    Task<Cliente?> ObtenerPorDocumentoAsync(string numeroDocumento);

    Task<Cliente> CrearAsync(Cliente cliente);

    Task ActualizarAsync(Cliente cliente);

    Task EliminarAsync(Cliente cliente);

    Task GuardarCambiosAsync();
}