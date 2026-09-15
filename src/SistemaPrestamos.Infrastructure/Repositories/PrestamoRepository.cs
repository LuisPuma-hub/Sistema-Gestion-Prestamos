using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class PrestamoRepository : IPrestamoRepository
{
    private readonly PrestamosDbContext _context;

    public PrestamoRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Prestamo>> ObtenerTodosAsync()
    {
        return await _context.Prestamos
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Garante)
            .OrderByDescending(x => x.FechaInicio)
            .ToListAsync();
    }

    public async Task<Prestamo?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.Prestamos
            .Include(x => x.Cliente)
            .Include(x => x.Garante)
            .Include(x => x.Pagos)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Prestamo>> ObtenerPorClienteAsync(
        Guid clienteId)
    {
        return await _context.Prestamos
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Garante)
            .Where(x => x.ClienteId == clienteId)
            .OrderByDescending(x => x.FechaInicio)
            .ToListAsync();
    }

    public async Task<Prestamo> CrearAsync(Prestamo prestamo)
    {
        await _context.Prestamos.AddAsync(prestamo);

        return prestamo;
    }

    public Task ActualizarAsync(Prestamo prestamo)
    {
        _context.Prestamos.Update(prestamo);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}