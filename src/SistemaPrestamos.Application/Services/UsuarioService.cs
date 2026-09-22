using Microsoft.AspNetCore.Identity;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly PasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(
        IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = new PasswordHasher<Usuario>();
    }

    public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync()
    {
        var usuarios = await _usuarioRepository.ObtenerTodosAsync();

        return usuarios.Select(MapearDto).ToList();
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(Guid id)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario is null)
            return null;

        return MapearDto(usuario);
    }

    public async Task<UsuarioDto> CrearAsync(
        CrearUsuarioDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombres))
            throw new InvalidOperationException(
                "Los nombres son obligatorios.");

        if (string.IsNullOrWhiteSpace(dto.Apellidos))
            throw new InvalidOperationException(
                "Los apellidos son obligatorios.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new InvalidOperationException(
                "El email es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new InvalidOperationException(
                "La contraseña es obligatoria.");

        if (dto.Password.Length < 8)
            throw new InvalidOperationException(
                "La contraseña debe tener al menos 8 caracteres.");

        var email = dto.Email.Trim().ToLowerInvariant();

        var usuarioExistente =
            await _usuarioRepository.ObtenerPorEmailAsync(email);

        if (usuarioExistente is not null)
            throw new InvalidOperationException(
                "Ya existe un usuario con ese email.");

        var rol = dto.Rol.Trim();

        if (rol != "Administrador" &&
            rol != "Cobrador")
        {
            throw new InvalidOperationException(
                "El rol debe ser Administrador o Cobrador.");
        }

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombres = dto.Nombres.Trim(),
            Apellidos = dto.Apellidos.Trim(),
            Email = email,
            Rol = rol,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        usuario.PasswordHash =
            _passwordHasher.HashPassword(
                usuario,
                dto.Password);

        await _usuarioRepository.CrearAsync(usuario);
        await _usuarioRepository.GuardarCambiosAsync();

        return MapearDto(usuario);
    }

    public async Task CambiarClaveAsync(
        Guid id,
        string actual,
        string nueva)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario is null)
            throw new InvalidOperationException(
                "El usuario no existe.");

        var verificacion = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            actual ?? string.Empty);

        if (verificacion == PasswordVerificationResult.Failed)
            throw new InvalidOperationException(
                "La contraseña actual es incorrecta.");

        await GuardarNuevaClaveAsync(usuario, nueva);
    }

    public async Task ResetearClaveAsync(Guid id, string nueva)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario is null)
            throw new InvalidOperationException(
                "El usuario no existe.");

        await GuardarNuevaClaveAsync(usuario, nueva);
    }

    public async Task<bool> EliminarAsync(Guid id, Guid actorId)
    {
        if (id == actorId)
            throw new InvalidOperationException(
                "No puedes eliminar tu propio usuario.");

        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario is null)
            return false;

        await _usuarioRepository.EliminarAsync(usuario);
        await _usuarioRepository.GuardarCambiosAsync();

        return true;
    }

    private async Task GuardarNuevaClaveAsync(
        Usuario usuario,
        string nueva)
    {
        if (string.IsNullOrWhiteSpace(nueva) || nueva.Length < 8)
            throw new InvalidOperationException(
                "La contraseña debe tener al menos 8 caracteres.");

        usuario.PasswordHash = _passwordHasher.HashPassword(
            usuario,
            nueva);

        await _usuarioRepository.ActualizarAsync(usuario);
        await _usuarioRepository.GuardarCambiosAsync();
    }

    private static UsuarioDto MapearDto(
        Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombres = usuario.Nombres,
            Apellidos = usuario.Apellidos,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Activo = usuario.Activo,
            FechaCreacion = usuario.FechaCreacion
        };
    }
}