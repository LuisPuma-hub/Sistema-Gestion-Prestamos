using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface INotaCompromisoRepository
{
    Task<NotaCompromiso?> ObtenerPorIdAsync(Guid id);

    Task<IEnumerable<NotaCompromiso>> ObtenerPorClienteAsync(
        Guid clienteId);

    Task<IEnumerable<NotaCompromiso>> ObtenerPendientesAsync();

    Task CrearAsync(NotaCompromiso compromiso);

    Task ActualizarAsync(NotaCompromiso compromiso);

    Task GuardarCambiosAsync();
}
