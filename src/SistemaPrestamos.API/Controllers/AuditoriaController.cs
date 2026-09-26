using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    // GET: api/auditoria?top=50
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditoriaDto>>> Obtener(
        [FromQuery] int top = 50)
    {
        return Ok(await _auditoriaService.ObtenerRecientesAsync(top));
    }
}
