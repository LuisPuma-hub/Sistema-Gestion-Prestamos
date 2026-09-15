using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private readonly PasswordHasher<Usuario> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
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

        return new LoginResponseDto
        {
            Token = token,
            UsuarioId = usuario.Id,
            Nombres = usuario.Nombres,
            Apellidos = usuario.Apellidos,
            Email = usuario.Email,
            Rol = usuario.Rol,
            ExpiraEn = DateTime.UtcNow.AddMinutes(
                expirationMinutes)
        };
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