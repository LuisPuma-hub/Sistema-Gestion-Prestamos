using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface ICompromisoService
{
    Task<CompromisoDto> CrearAsync(
        Guid clienteId,
        CrearCompromisoDto dto,
        Guid? creadoPor);

    Task<IEnumerable<CompromisoDto>> ListarPorClienteAsync(
        Guid clienteId);

    Task<bool> AnularAsync(Guid id);

    // Marca cumplidos los pendientes del cliente en esa fecha
    // (solo con pago registrado ese día; respeta préstamo).
    Task<int> CumplirPorPagoAsync(
        Guid clienteId, Guid prestamoId, DateTime fecha);

    // Pendientes con fecha igual a hoy.
    Task<IEnumerable<CompromisoDto>> PendientesDeHoyAsync(DateTime hoy);

    // Pasa a Vencida lo pendiente con fecha pasada.
    Task<int> MarcarVencidosAsync(DateTime hoy);

    // Revierte lo cumplido en esa fecha (anulación del pago).
    Task<int> RevertirPorAnulacionAsync(
        Guid clienteId, Guid prestamoId, DateTime fecha);
}
