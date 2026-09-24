using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IReglaNotificacionRepository
{
    Task<IEnumerable<ReglaNotificacion>> ObtenerTodasAsync();

    Task<ReglaNotificacion?> ObtenerPorIdAsync(Guid id);

    Task<ReglaNotificacion> CrearAsync(ReglaNotificacion regla);

    Task ActualizarAsync(ReglaNotificacion regla);

    Task EliminarAsync(ReglaNotificacion regla);

    Task GuardarCambiosAsync();
}
