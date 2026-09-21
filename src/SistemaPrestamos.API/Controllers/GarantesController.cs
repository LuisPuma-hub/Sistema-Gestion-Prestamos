using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GarantesController : ControllerBase
{
    private readonly IGaranteService _garanteService;

    public GarantesController(IGaranteService garanteService)
    {
        _garanteService = garanteService;
    }

    // GET: api/garantes/cliente/{clienteId}
    [HttpGet("cliente/{clienteId:guid}")]
    public async Task<ActionResult<IEnumerable<GaranteDto>>> ObtenerPorCliente(
        Guid clienteId)
    {
        var garantes = await _garanteService
            .ObtenerPorClienteAsync(clienteId);

        return Ok(garantes);
    }

    // GET: api/garantes/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GaranteDto>> ObtenerPorId(Guid id)
    {
        var garante = await _garanteService.ObtenerPorIdAsync(id);

        if (garante is null)
        {
            return NotFound(new
            {
                mensaje = "Garante no encontrado."
            });
        }

        return Ok(garante);
    }

    // POST: api/garantes
    [HttpPost]
    public async Task<ActionResult<GaranteDto>> Crear(
        [FromBody] CrearGaranteDto dto)
    {
        try
        {
            var garante = await _garanteService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = garante.Id },
                garante);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // PUT: api/garantes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] CrearGaranteDto dto)
    {
        try
        {
            var actualizado = await _garanteService
                .ActualizarAsync(id, dto);

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "Garante no encontrado."
                });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // DELETE: api/garantes/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        var eliminado = await _garanteService.EliminarAsync(id);

        if (!eliminado)
        {
            return NotFound(new
            {
                mensaje = "Garante no encontrado."
            });
        }

        return NoContent();
    }
}
