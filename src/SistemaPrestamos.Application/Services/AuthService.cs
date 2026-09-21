using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly PasswordHasher<Usuario> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IRefreshTokenRepository refreshRepository,
        IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _refreshRepository = refreshRepository;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<Usuario>();
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new InvalidOperationException(
                "El email es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new InvalidOperationException(
                "La contraseña es obligatoria.");
        }

        var email = dto.Email.Trim().ToLowerInvariant();

        var usuario = await _usuarioRepository
            .ObtenerPorEmailAsync(email);

        if (usuario is null)
        {
            throw new InvalidOperationException(
                "Credenciales incorrectas.");
        }

        if (!usuario.Activo)
        {
            throw new InvalidOperationException(
                "El usuario está inactivo.");
        }

        var resultado = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            dto.Password);

        if (resultado == PasswordVerificationResult.Failed)
        {
            throw new InvalidOperationException(
                "Credenciales incorrectas.");
        }

        var token = GenerarToken(usuario);

        var expirationMinutes = ObtenerExpirationMinutes();

        var refresh = await CrearRefreshTokenAsync(usuario.Id);

        return new LoginResponseDto
        {
            Token = token,
            UsuarioId = usuario.Id,
            Nombres = usuario.Nombres,
            Apellidos = usuario.Apellidos,
            Email = usuario.Email,
            Rol = usuario.Rol,
            ExpiraEn = DateTime.UtcNow.AddMinutes(
                expirationMinutes),
            RefreshToken = refresh.Token,
            RefreshExpiraEn = refresh.ExpiraEn
        };
    }

    public async Task<LoginResponseDto> RefreshAsync(
        string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new InvalidOperationException(
                "Sesión inválida.");
        }

        var guardado = await _refreshRepository
            .ObtenerPorTokenAsync(refreshToken.Trim());

        if (guardado is null || !guardado.Vigente)
        {
            throw new InvalidOperationException(
                "Sesión expirada. Inicie sesión nuevamente.");
        }

        var usuario = guardado.Usuario
            ?? await _usuarioRepository.ObtenerPorIdAsync(
                guardado.UsuarioId);

        if (usuario is null || !usuario.Activo)
        {
            throw new InvalidOperationException(
                "Usuario no disponible.");
        }

        // Rotación: el refresh usado se revoca.
        guardado.FechaRevocacion = DateTime.UtcNow;
        await _refreshRepository.ActualizarAsync(guardado);

        var token = GenerarToken(usuario);
        var expirationMinutes = ObtenerExpirationMinutes();
        var nuevo = await CrearRefreshTokenAsync(usuario.Id);

        await _refreshRepository.GuardarCambiosAsync();

        return new LoginResponseDto
        {
            Token = token,
            UsuarioId = usuario.Id,
            Nombres = usuario.Nombres,
            Apellidos = usuario.Apellidos,
            Email = usuario.Email,
            Rol = usuario.Rol,
            ExpiraEn = DateTime.UtcNow.AddMinutes(
                expirationMinutes),
            RefreshToken = nuevo.Token,
            RefreshExpiraEn = nuevo.ExpiraEn
        };
    }

    public async Task RevocarAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        var guardado = await _refreshRepository
            .ObtenerPorTokenAsync(refreshToken.Trim());

        if (guardado is null || guardado.Revocado)
        {
            return;
        }

        guardado.FechaRevocacion = DateTime.UtcNow;

        await _refreshRepository.ActualizarAsync(guardado);
        await _refreshRepository.GuardarCambiosAsync();
    }

    private async Task<RefreshToken> CrearRefreshTokenAsync(Guid usuarioId)
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        var dias = _configuration
            .GetValue<int?>("Jwt:RefreshExpirationDays")
            ?? 7;

        var refresh = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            Token = Convert.ToBase64String(bytes),
            ExpiraEn = DateTime.UtcNow.AddDays(dias),
            FechaCreacion = DateTime.UtcNow
        };

        await _refreshRepository.CrearAsync(refresh);
        await _refreshRepository.GuardarCambiosAsync();

        return refresh;
    }

    private string GenerarToken(Usuario usuario)
    {
        var key = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "La configuración Jwt:Key no está definida.");
        }

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var expirationMinutes = ObtenerExpirationMinutes();

        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                usuario.Id.ToString()),

            new(
                JwtRegisteredClaimNames.Email,
                usuario.Email),

            new(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()),

            new(
                ClaimTypes.Name,
                $"{usuario.Nombres} {usuario.Apellidos}"),

            new(
                ClaimTypes.Role,
                usuario.Rol)
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    private int ObtenerExpirationMinutes()
    {
        return _configuration
            .GetValue<int?>("Jwt:ExpirationMinutes")
            ?? 120;
    }
}