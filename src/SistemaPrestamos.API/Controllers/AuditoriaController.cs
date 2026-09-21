using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class AuditoriaController : ControllerBase
{
    private readonly PrestamosDbContext _context;

    public AuditoriaController(PrestamosDbContext context)
    {
        _context = context;
    }

    // GET: api/auditoria?top=50
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Auditoria>>> Obtener(
        [FromQuery] int top = 50)
    {
        if (top is < 1 or > 500)
        {
            top = 50;
        }

        var registros = await _context.Auditorias
            .AsNoTracking()
            .OrderByDescending(x => x.Fecha)
            .Take(top)
            .ToListAsync();

        return Ok(registros);
    }
}
