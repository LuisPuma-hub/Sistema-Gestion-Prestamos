using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificacionesController : ControllerBase
{
    private readonly INotificacionService _notificacionService;

    public NotificacionesController(INotificacionService notificacionService)
    {
        _notificacionService = notificacionService;
    }

    public class ProbarDto
    {
        public string Titulo { get; set; } = "Prueba";
        public string Cuerpo { get; set; } = "Notificación de prueba.";
    }

    // POST: api/notificaciones/probar
    [HttpPost("probar")]
    public async Task<ActionResult<object>> Probar(
        [FromBody] ProbarDto dto)
    {
        var idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(idTexto, out var usuarioId))
        {
            return Unauthorized(new
            {
                mensaje = "No se pudo identificar al usuario."
            });
        }

        var enviados = await _notificacionService.EnviarAUsuarioAsync(
            usuarioId,
            string.IsNullOrWhiteSpace(dto.Titulo) ? "Prueba" : dto.Titulo,
            string.IsNullOrWhiteSpace(dto.Cuerpo) ? "Notificación de prueba." : dto.Cuerpo);

        return Ok(new
        {
            mensaje = $"Notificación enviada a {enviados} dispositivo(s)."
        });
    }
}
