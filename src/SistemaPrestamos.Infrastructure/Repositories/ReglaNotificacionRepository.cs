using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class ReglaNotificacionRepository : IReglaNotificacionRepository
{
    private readonly PrestamosDbContext _context;

    public ReglaNotificacionRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReglaNotificacion>> ObtenerTodasAsync()
    {
        return await _context.ReglasNotificacion
            .AsNoTracking()
            .OrderBy(x => x.Hora)
            .ToListAsync();
    }

    public async Task<ReglaNotificacion?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.ReglasNotificacion
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ReglaNotificacion> CrearAsync(ReglaNotificacion regla)
    {
        await _context.ReglasNotificacion.AddAsync(regla);

        return regla;
    }

    public Task ActualizarAsync(ReglaNotificacion regla)
    {
        var rastreado = _context.ChangeTracker.Entries<ReglaNotificacion>()
            .FirstOrDefault(e => e.Entity.Id == regla.Id);

        if (rastreado is not null)
        {
            rastreado.CurrentValues.SetValues(regla);
        }
        else
        {
            _context.ReglasNotificacion.Update(regla);
        }

        return Task.CompletedTask;
    }

    public Task EliminarAsync(ReglaNotificacion regla)
    {
        var rastreado = _context.ChangeTracker.Entries<ReglaNotificacion>()
            .FirstOrDefault(e => e.Entity.Id == regla.Id);

        if (rastreado is not null)
        {
            rastreado.State = EntityState.Deleted;
        }
        else
        {
            _context.ReglasNotificacion.Remove(regla);
        }

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
