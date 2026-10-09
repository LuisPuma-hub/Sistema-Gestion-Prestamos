using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class FondoRepository : IFondoRepository
{
    private readonly PrestamosDbContext _context;

    public FondoRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FondoMovimiento>> ObtenerTodosAsync()
    {
        return await _context.FondoMovimientos
            .AsNoTracking()
            .OrderByDescending(x => x.Fecha)
            .ToListAsync();
    }

    public async Task<FondoMovimiento?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.FondoMovimientos
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<FondoMovimiento> CrearAsync(
        FondoMovimiento movimiento)
    {
        await _context.FondoMovimientos.AddAsync(movimiento);
        return movimiento;
    }

    public Task EliminarAsync(FondoMovimiento movimiento)
    {
        var rastreado = _context.ChangeTracker.Entries<FondoMovimiento>()
            .FirstOrDefault(e => e.Entity.Id == movimiento.Id);

        if (rastreado is not null)
            rastreado.State = EntityState.Deleted;
        else
            _context.FondoMovimientos.Remove(movimiento);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
