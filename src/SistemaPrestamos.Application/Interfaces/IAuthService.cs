using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);

    Task<LoginResponseDto> RefreshAsync(string refreshToken);

    Task RevocarAsync(string refreshToken);
}