using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DispositivosController : ControllerBase
{
    private readonly IDispositivoService _dispositivoService;

    public DispositivosController(IDispositivoService dispositivoService)
    {
        _dispositivoService = dispositivoService;
    }

    // POST: api/dispositivos
    [HttpPost]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistrarDispositivoDto dto)
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

            await _dispositivoService.RegistrarAsync(
                usuarioId,
                dto.Token,
                dto.Plataforma);

            return Ok(new
            {
                mensaje = "Dispositivo registrado."
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
