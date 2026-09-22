using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUsuarioService _usuarioService;

    public AuthController(
        IAuthService authService,
        IUsuarioService usuarioService)
    {
        _authService = authService;
        _usuarioService = usuarioService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginDto dto)
    {
        try
        {
            var respuesta = await _authService.LoginAsync(dto);

            return Ok(respuesta);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponseDto>> Refresh(
        [FromBody] RefreshDto dto)
    {
        try
        {
            var respuesta = await _authService.RefreshAsync(
                dto.RefreshToken);

            return Ok(respuesta);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshDto dto)
    {
        await _authService.RevocarAsync(dto.RefreshToken);

        return Ok(new
        {
            mensaje = "Sesión cerrada."
        });
    }

    // POST: api/auth/cambiar-clave (usuario autenticado, exige actual)
    [HttpPost("cambiar-clave")]
    [Authorize]
    public async Task<IActionResult> CambiarClave(
        [FromBody] CambiarClaveDto dto)
    {
        try
        {
            var idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(idTexto, out var usuarioId))
            {
                return Unauthorized(new
                {
                    mensaje = "No se pudo identificar al usuario."
                });
            }

            await _usuarioService.CambiarClaveAsync(
                usuarioId,
                dto.Actual ?? string.Empty,
                dto.Nueva);

            return Ok(new
            {
                mensaje = "Contraseña actualizada."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }
}