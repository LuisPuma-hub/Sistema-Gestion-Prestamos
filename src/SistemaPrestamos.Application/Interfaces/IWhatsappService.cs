using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IWhatsappService
{
    Task<MensajeWhatsapp> EnviarPlantillaAsync(
        Guid? clienteId,
        Guid? prestamoId,
        string numeroDestino,
        string nombrePlantilla,
        string contenido,
        string idioma = "es_PE");

    Task<MensajeWhatsapp> EnviarTextoAsync(
        Guid? clienteId,
        Guid? prestamoId,
        string numeroDestino,
        string texto);

    Task<MensajeWhatsapp> EnviarRecordatorioAsync(
        Guid clienteId,
        Guid? prestamoId);

    Task<IEnumerable<MensajeWhatsapp>> ObtenerHistorialAsync(
        Guid clienteId);
}
