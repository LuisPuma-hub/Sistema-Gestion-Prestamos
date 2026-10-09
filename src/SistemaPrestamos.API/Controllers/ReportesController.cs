using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportesController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    // GET: api/reportes/cobranza?fecha=2026-09-27
    [HttpGet("cobranza")]
    public async Task<ActionResult<CobranzaDto>> Cobranza(
        [FromQuery] DateTime? fecha)
    {
        var dto = await _reporteService.CobranzaDelDiaAsync(fecha);

        return Ok(dto);
    }

    // GET: api/reportes/cartera
    [HttpGet("cartera")]
    public async Task<ActionResult<CarteraDto>> Cartera()
    {
        var dto = await _reporteService.CarteraAsync();

        return Ok(dto);
    }

    // GET: api/reportes/cartera/csv
    [HttpGet("cartera/csv")]
    public async Task<IActionResult> CarteraCsv()
    {
        var dto = await _reporteService.CarteraAsync();

        var csv = new StringBuilder();
        csv.AppendLine("cliente,capital_pendiente,intereses,total,estado");

        foreach (var fila in dto.Filas)
        {
            csv.AppendLine(
                $"\"{fila.Cliente}\"," +
                $"{fila.CapitalPendiente:N2}," +
                $"{fila.InteresesPendientes:N2}," +
                $"{fila.Total:N2},{fila.Estado}");
        }

        var bytes = Encoding.UTF8.GetBytes("\uFEFF" + csv.ToString());

        return File(bytes, "text/csv", "cartera.csv");
    }

    // GET: api/reportes/ingresos?desde=2026-09-01&hasta=2026-09-27
    [HttpGet("ingresos")]
    public async Task<ActionResult<List<IngresoDiaDto>>> Ingresos(
        [FromQuery] DateTime desde,
        [FromQuery] DateTime hasta)
    {
        try
        {
            var lista = await _reporteService.IngresosAsync(
                desde, hasta);

            return Ok(lista);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // GET: api/reportes/capital
    [HttpGet("capital")]
    public async Task<ActionResult<CapitalDto>> Capital()
    {
        var dto = await _reporteService.CapitalAsync();

        return Ok(dto);
    }

    // GET: api/reportes/fondo
    [HttpGet("fondo")]
    public async Task<ActionResult<List<FondoMovimientoDto>>> Movimientos()
    {
        var lista = await _reporteService.MovimientosAsync();

        return Ok(lista);
    }

    // POST: api/reportes/fondo (solo ADMIN)
    [HttpPost("fondo")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<FondoMovimientoDto>> RegistrarMovimiento(
        [FromBody] CrearFondoMovimientoDto dto)
    {
        try
        {
            var creado = await _reporteService
                .RegistrarMovimientoAsync(dto);

            return Ok(creado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // DELETE: api/reportes/fondo/{id} (solo ADMIN)
    [HttpDelete("fondo/{id:guid}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> EliminarMovimiento(Guid id)
    {
        var eliminado = await _reporteService
            .EliminarMovimientoAsync(id);

        if (!eliminado)
            return NotFound(new
            {
                mensaje = "Movimiento no encontrado."
            });

        return NoContent();
    }
}
