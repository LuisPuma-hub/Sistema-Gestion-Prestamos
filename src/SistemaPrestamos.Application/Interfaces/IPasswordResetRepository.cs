using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IPasswordResetRepository
{
    Task<PasswordReset?> ObtenerPorTokenAsync(string token);

    Task<PasswordReset> CrearAsync(PasswordReset reset);

    Task GuardarCambiosAsync();
}
