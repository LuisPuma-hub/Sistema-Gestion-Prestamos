using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoService _prestamoService;
    private readonly IPeriodoInteresService _periodoInteresService;

    public PrestamosController(
        IPrestamoService prestamoService,
        IPeriodoInteresService periodoInteresService)
    {
        _prestamoService = prestamoService;
        _periodoInteresService = periodoInteresService;
    }

    // GET: api/prestamos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrestamoDto>>> ObtenerTodos()
    {
        var prestamos = await _prestamoService.ObtenerTodosAsync();

        return Ok(prestamos);
    }

    // GET: api/prestamos/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PrestamoDto>> ObtenerPorId(Guid id)
    {
        var prestamo = await _prestamoService.ObtenerPorIdAsync(id);

        if (prestamo is null)
        {
            return NotFound(new
            {
                mensaje = "Préstamo no encontrado."
            });
        }

        return Ok(prestamo);
    }

    // GET: api/prestamos/cliente/{clienteId}
    [HttpGet("cliente/{clienteId:guid}")]
    public async Task<ActionResult<IEnumerable<PrestamoDto>>> ObtenerPorCliente(
        Guid clienteId)
    {
        try
        {
            var prestamos = await _prestamoService
                .ObtenerPorClienteAsync(clienteId);

            return Ok(prestamos);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
    }

    // POST: api/prestamos
    [HttpPost]
    public async Task<ActionResult<PrestamoDto>> Crear(
        [FromBody] CrearPrestamoDto dto)
    {
        try
        {
            var prestamo = await _prestamoService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = prestamo.Id },
                prestamo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // PATCH: api/prestamos/{id}/aprobar (solo ADMIN)
    [HttpPatch("{id:guid}/aprobar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Aprobar(Guid id)
    {
        try
        {
            var aprobado = await _prestamoService.AprobarAsync(id);

            if (!aprobado)
            {
                return NotFound(new
                {
                    mensaje = "Préstamo no encontrado."
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
    // PATCH: api/prestamos/{id}/anular (solo ADMIN)
    [HttpPatch("{id:guid}/anular")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Anular(
        Guid id,
        [FromBody] AnularPrestamoDto dto)
    {
        try
        {
            var anulado = await _prestamoService.AnularAsync(
                id,
                dto.Motivo);

            if (!anulado)
            {
                return NotFound(new
                {
                    mensaje = "Préstamo no encontrado."
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

    [HttpPost("{id:guid}/periodos")]
    public async Task<IActionResult> GenerarPeriodos(
        Guid id,
        [FromQuery] DateTime fechaReferencia)
    {
        try
        {
            await _prestamoService.CrearPeriodosPendientesAsync(
                id,
                fechaReferencia);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // GET: api/prestamos/{id}/periodos
    [HttpGet("{id:guid}/periodos")]
    public async Task<ActionResult<IEnumerable<PeriodoInteresDto>>> ObtenerPeriodos(
        Guid id)
    {
        try
        {
            var periodos = await _periodoInteresService
                .ObtenerPorPrestamoAsync(id);

            return Ok(periodos);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
    }
}