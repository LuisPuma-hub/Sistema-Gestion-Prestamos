using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class CompromisosController : ControllerBase
{
    private readonly ICompromisoService _compromisoService;

    public CompromisosController(
        ICompromisoService compromisoService)
    {
        _compromisoService = compromisoService;
    }

    // POST: api/compromisos/{clienteId}
    [HttpPost("{clienteId:guid}")]
    public async Task<ActionResult<CompromisoDto>> Crear(
        Guid clienteId,
        [FromBody] CrearCompromisoDto dto)
    {
        try
        {
            var idTexto = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            Guid.TryParse(idTexto, out var actorId);

            var creado = await _compromisoService.CrearAsync(
                clienteId,
                dto,
                actorId == Guid.Empty ? null : actorId);

            return Ok(creado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // GET: api/compromisos/cliente/{clienteId}
    [HttpGet("cliente/{clienteId:guid}")]
    public async Task<ActionResult<IEnumerable<CompromisoDto>>> Listar(
        Guid clienteId)
    {
        return Ok(await _compromisoService
            .ListarPorClienteAsync(clienteId));
    }

    // DELETE: api/compromisos/{id}
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Anular(Guid id)
    {
        try
        {
            return await _compromisoService.AnularAsync(id)
                ? NoContent()
                : NotFound(new
                {
                    mensaje = "El compromiso no existe."
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
