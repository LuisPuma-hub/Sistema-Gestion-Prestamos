using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> ObtenerPorTokenAsync(string token);

    Task<RefreshToken> CrearAsync(RefreshToken refreshToken);

    Task ActualizarAsync(RefreshToken refreshToken);

    Task GuardarCambiosAsync();
}
