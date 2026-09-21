using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IGaranteRepository
{
    Task<IEnumerable<Garante>> ObtenerPorClienteAsync(Guid clienteId);

    Task<Garante?> ObtenerPorIdAsync(Guid id);

    Task<Garante> CrearAsync(Garante garante);

    Task ActualizarAsync(Garante garante);

    Task EliminarAsync(Garante garante);

    Task GuardarCambiosAsync();
}
