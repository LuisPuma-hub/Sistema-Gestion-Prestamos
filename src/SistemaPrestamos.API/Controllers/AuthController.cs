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

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
}