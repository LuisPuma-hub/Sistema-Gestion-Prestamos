using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class NotaCompromisoRepository : INotaCompromisoRepository
{
    private readonly PrestamosDbContext _context;

    public NotaCompromisoRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<NotaCompromiso?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.NotasCompromiso
            .Include(x => x.Cliente)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<NotaCompromiso>> ObtenerPorClienteAsync(
        Guid clienteId)
    {
        return await _context.NotasCompromiso
            .Include(x => x.Cliente)
            .Where(x => x.ClienteId == clienteId)
            .OrderByDescending(x => x.FechaCompromiso)
            .ToListAsync();
    }

    public async Task<IEnumerable<NotaCompromiso>> ObtenerPendientesAsync()
    {
        return await _context.NotasCompromiso
            .Include(x => x.Cliente)
            .Where(x => x.Estado == "Pendiente")
            .ToListAsync();
    }

    public async Task CrearAsync(NotaCompromiso compromiso)
    {
        await _context.NotasCompromiso.AddAsync(compromiso);
    }

    public async Task ActualizarAsync(NotaCompromiso compromiso)
    {
        _context.NotasCompromiso.Update(compromiso);
        await Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
