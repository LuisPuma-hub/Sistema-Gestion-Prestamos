using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class MorosidadRepository : IMorosidadRepository
{
    private readonly PrestamosDbContext _context;

    public MorosidadRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<Morosidad?> ObtenerPorPrestamoAsync(
        Guid prestamoId)
    {
        return await _context.Morosidades
            .FirstOrDefaultAsync(x => x.PrestamoId == prestamoId);
    }

    public async Task<Morosidad?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.Morosidades
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Morosidad> CrearAsync(
        Morosidad morosidad)
    {
        await _context.Morosidades.AddAsync(morosidad);

        return morosidad;
    }

    public Task ActualizarAsync(Morosidad morosidad)
    {
        _context.Morosidades.Update(morosidad);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}