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
        string idioma = "es_PE",
        List<string>? parametros = null);

    Task<MensajeWhatsapp> EnviarTextoAsync(
        Guid? clienteId,
        Guid? prestamoId,
        string numeroDestino,
        string texto);

    Task<MensajeWhatsapp> EnviarRecordatorioAsync(
        Guid clienteId,
        Guid? prestamoId);

    Task<MensajeWhatsapp> EnviarPlantillaCatalogoAsync(
        Guid clienteId,
        Guid? prestamoId,
        string plantilla);

    Task<IEnumerable<MensajeWhatsapp>> ObtenerHistorialAsync(
        Guid clienteId);
}
