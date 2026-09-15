using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class PagoRepository : IPagoRepository
{
    private readonly PrestamosDbContext _context;

    public PagoRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pago>> ObtenerTodosAsync()
    {
        return await _context.Pagos
            .AsNoTracking()
            .Include(x => x.Prestamo)
            .OrderByDescending(x => x.FechaPago)
            .ToListAsync();
    }

    public async Task<Pago?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.Pagos
            .Include(x => x.Prestamo)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Pago>> ObtenerPorPrestamoAsync(Guid prestamoId)
    {
        return await _context.Pagos
            .AsNoTracking()
            .Where(x => x.PrestamoId == prestamoId)
            .OrderByDescending(x => x.FechaPago)
            .ToListAsync();
    }

    public async Task<Pago> CrearAsync(Pago pago)
    {
        await _context.Pagos.AddAsync(pago);
        return pago;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}