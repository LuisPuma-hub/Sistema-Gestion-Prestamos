using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class EnvioNotificacionRepository : IEnvioNotificacionRepository
{
    private readonly PrestamosDbContext _context;

    public EnvioNotificacionRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<EnvioNotificacion> CrearAsync(EnvioNotificacion envio)
    {
        await _context.EnviosNotificacion.AddAsync(envio);

        return envio;
    }

    public async Task<bool> ExisteHoyAsync(
        Guid? reglaId,
        string evento,
        string canal,
        Guid? clienteId,
        Guid? prestamoId,
        Guid? usuarioId,
        DateTime hoy)
    {
        var inicio = hoy.Date;
        var fin = inicio.AddDays(1);

        return await _context.EnviosNotificacion
            .AsNoTracking()
            .AnyAsync(x =>
                x.FechaCreacion >= inicio &&
                x.FechaCreacion < fin &&
                x.Evento == evento &&
                x.Canal == canal &&
                x.ReglaId == reglaId &&
                x.ClienteId == clienteId &&
                x.PrestamoId == prestamoId &&
                x.UsuarioId == usuarioId &&
                x.Estado == "Enviado");
    }

    public async Task<IEnumerable<EnvioNotificacion>> ObtenerRecientesAsync(
        int top)
    {
        return await _context.EnviosNotificacion
            .AsNoTracking()
            .OrderByDescending(x => x.FechaCreacion)
            .Take(top)
            .ToListAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
