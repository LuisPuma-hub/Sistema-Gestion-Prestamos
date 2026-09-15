using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PagosController : ControllerBase
{
    private readonly IPagoService _pagoService;

    public PagosController(IPagoService pagoService)
    {
        _pagoService = pagoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PagoDto>>> ObtenerTodos()
    {
        var pagos = await _pagoService.ObtenerTodosAsync();

        return Ok(pagos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PagoDto>> ObtenerPorId(Guid id)
    {
        var pago = await _pagoService.ObtenerPorIdAsync(id);

        if (pago is null)
            return NotFound(new
            {
                mensaje = "Pago no encontrado."
            });

        return Ok(pago);
    }

    [HttpGet("prestamo/{prestamoId:guid}")]
    public async Task<ActionResult<IEnumerable<PagoDto>>> ObtenerPorPrestamo(
        Guid prestamoId)
    {
        try
        {
            var pagos = await _pagoService.ObtenerPorPrestamoAsync(prestamoId);

            return Ok(pagos);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<PagoDto>> Registrar(
        [FromBody] CrearPagoDto dto)
    {
        try
        {
            var pago = await _pagoService.RegistrarAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = pago.Id },
                pago);
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