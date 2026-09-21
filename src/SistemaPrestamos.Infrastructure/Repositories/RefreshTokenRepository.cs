using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly PrestamosDbContext _context;

    public RefreshTokenRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> ObtenerPorTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(x => x.Usuario)
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<RefreshToken> CrearAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        return refreshToken;
    }

    public Task ActualizarAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
