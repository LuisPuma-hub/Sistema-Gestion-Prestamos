using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class SaludController : ControllerBase
{
    // GET: api/salud (sin auth: usado por keep-alive y monitoreo)
    [HttpGet]
    public IActionResult Verificar()
    {
        return Ok(new
        {
            estado = "ok",
            fecha = DateTime.UtcNow
        });
    }
}
