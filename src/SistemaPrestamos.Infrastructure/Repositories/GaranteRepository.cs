using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class GaranteRepository : IGaranteRepository
{
    private readonly PrestamosDbContext _context;

    public GaranteRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Garante>> ObtenerPorClienteAsync(
        Guid clienteId)
    {
        return await _context.Garantes
            .AsNoTracking()
            .Where(x => x.ClienteId == clienteId)
            .OrderBy(x => x.Apellidos)
            .ThenBy(x => x.Nombres)
            .ToListAsync();
    }

    public async Task<Garante?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.Garantes
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Garante> CrearAsync(Garante garante)
    {
        await _context.Garantes.AddAsync(garante);
        return garante;
    }

    public Task ActualizarAsync(Garante garante)
    {
        _context.Garantes.Update(garante);
        return Task.CompletedTask;
    }

    public Task EliminarAsync(Garante garante)
    {
        var rastreado = _context.ChangeTracker.Entries<Garante>()
            .FirstOrDefault(e => e.Entity.Id == garante.Id);

        if (rastreado is not null)
        {
            rastreado.State = EntityState.Deleted;
        }
        else
        {
            _context.Garantes.Remove(garante);
        }

        return Task.CompletedTask;
    }

    public async Task<int> EliminarPorClienteAsync(Guid clienteId)
    {
        return await _context.Garantes
            .Where(x => x.ClienteId == clienteId)
            .ExecuteDeleteAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
