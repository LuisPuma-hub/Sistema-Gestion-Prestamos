using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class PeriodoInteresRepository : IPeriodoInteresRepository
{
    private readonly PrestamosDbContext _context;

    public PeriodoInteresRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<PeriodoInteres> CrearAsync(
        PeriodoInteres periodo)
    {
        await _context.PeriodosInteres.AddAsync(periodo);

        return periodo;
    }

    public async Task<IEnumerable<PeriodoInteres>>
        ObtenerPorPrestamoAsync(Guid prestamoId)
    {
        return await _context.PeriodosInteres
            .Where(x => x.PrestamoId == prestamoId)
            .OrderBy(x => x.FechaInicio)
            .ToListAsync();
    }

    public async Task<PeriodoInteres?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.PeriodosInteres
            .Include(x => x.Prestamo)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task EliminarAsync(PeriodoInteres periodo)
    {
        var rastreado = _context.ChangeTracker.Entries<PeriodoInteres>()
            .FirstOrDefault(e => e.Entity.Id == periodo.Id);

        if (rastreado is not null)
        {
            rastreado.State = EntityState.Deleted;
        }
        else
        {
            _context.PeriodosInteres.Remove(periodo);
        }

        return Task.CompletedTask;
    }

    public async Task<int> EliminarPorPrestamoAsync(Guid prestamoId)
    {
        return await _context.PeriodosInteres
            .Where(x => x.PrestamoId == prestamoId)
            .ExecuteDeleteAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}