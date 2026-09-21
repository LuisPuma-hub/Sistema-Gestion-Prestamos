using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class MensajeWhatsappRepository : IMensajeWhatsappRepository
{
    private readonly PrestamosDbContext _context;

    public MensajeWhatsappRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MensajeWhatsapp>> ObtenerPorClienteAsync(
        Guid clienteId)
    {
        return await _context.MensajesWhatsapp
            .AsNoTracking()
            .Where(x => x.ClienteId == clienteId)
            .OrderByDescending(x => x.FechaCreacion)
            .ToListAsync();
    }

    public async Task<MensajeWhatsapp> CrearAsync(MensajeWhatsapp mensaje)
    {
        await _context.MensajesWhatsapp.AddAsync(mensaje);
        return mensaje;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
