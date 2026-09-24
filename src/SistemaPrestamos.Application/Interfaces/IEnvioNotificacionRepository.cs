using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IEnvioNotificacionRepository
{
    Task<EnvioNotificacion> CrearAsync(EnvioNotificacion envio);

    Task<bool> ExisteHoyAsync(
        Guid? reglaId,
        string evento,
        string canal,
        Guid? clienteId,
        Guid? prestamoId,
        Guid? usuarioId,
        DateTime hoy);

    Task<IEnumerable<EnvioNotificacion>> ObtenerRecientesAsync(int top);

    Task GuardarCambiosAsync();
}
