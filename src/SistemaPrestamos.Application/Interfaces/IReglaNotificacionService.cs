using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IReglaNotificacionService
{
    Task<IEnumerable<ReglaNotificacionDto>> ListarAsync();

    Task<ReglaNotificacionDto?> ObtenerAsync(Guid id);

    Task<ReglaNotificacionDto> CrearAsync(CrearReglaDto dto);

    Task<ReglaNotificacionDto?> ActualizarAsync(
        Guid id,
        ActualizarReglaDto dto);

    Task<ReglaNotificacionDto?> CambiarEstadoAsync(
        Guid id,
        bool activa);

    Task<bool> EliminarAsync(Guid id);

    Task<(bool Exito, string Mensaje)> ProbarAsync(
        Guid id,
        Guid usuarioId,
        Guid? clienteId = null,
        Guid? prestamoId = null);

    Task<IEnumerable<EnvioNotificacionDto>> ObtenerEnviosAsync(int top);
}
