using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IMensajeWhatsappRepository
{
    Task<IEnumerable<MensajeWhatsapp>> ObtenerPorClienteAsync(Guid clienteId);

    Task<MensajeWhatsapp> CrearAsync(MensajeWhatsapp mensaje);

    Task EliminarAsync(MensajeWhatsapp mensaje);

    Task<int> EliminarPorClienteAsync(Guid clienteId);

    Task GuardarCambiosAsync();
}
