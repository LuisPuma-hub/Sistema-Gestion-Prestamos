using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class AuditoriaRepository : IAuditoriaRepository
{
    private readonly PrestamosDbContext _context;

    public AuditoriaRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Auditoria>> ObtenerRecientesAsync(int top)
    {
        return await _context.Auditorias
            .AsNoTracking()
            .OrderByDescending(x => x.Fecha)
            .Take(top)
            .ToListAsync();
    }
}
