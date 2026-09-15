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
}