using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class PasswordResetRepository : IPasswordResetRepository
{
    private readonly PrestamosDbContext _context;

    public PasswordResetRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<PasswordReset?> ObtenerPorTokenAsync(string token)
    {
        return await _context.PasswordResets
            .Include(x => x.Usuario)
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<PasswordReset> CrearAsync(PasswordReset reset)
    {
        await _context.PasswordResets.AddAsync(reset);
        return reset;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
