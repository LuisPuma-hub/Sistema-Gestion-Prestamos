using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class DispositivoRepository : IDispositivoRepository
{
    private readonly PrestamosDbContext _context;

    public DispositivoRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<Dispositivo?> ObtenerPorTokenAsync(string token)
    {
        return await _context.Dispositivos
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<Dispositivo?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.Dispositivos
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Dispositivo>> ObtenerPorUsuarioAsync(
        Guid usuarioId)
    {
        return await _context.Dispositivos
            .AsNoTracking()
            .Where(x => x.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Dispositivo>> ObtenerTodosAsync()
    {
        return await _context.Dispositivos
            .AsNoTracking()
            .OrderByDescending(x => x.FechaActualizacion)
            .ToListAsync();
    }

    public async Task<Dispositivo> CrearAsync(Dispositivo dispositivo)
    {
        await _context.Dispositivos.AddAsync(dispositivo);
        return dispositivo;
    }

    public Task ActualizarAsync(Dispositivo dispositivo)
    {
        _context.Dispositivos.Update(dispositivo);
        return Task.CompletedTask;
    }

    public Task EliminarAsync(Dispositivo dispositivo)
    {
        _context.Dispositivos.Remove(dispositivo);
        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
