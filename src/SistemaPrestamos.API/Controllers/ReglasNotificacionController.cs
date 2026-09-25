using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.API.Jobs;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Application.Services;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class ReglasNotificacionController : ControllerBase
{
    private readonly IReglaNotificacionService _reglaService;

    public ReglasNotificacionController(
        IReglaNotificacionService reglaService)
    {
        _reglaService = reglaService;
    }

    // GET: api/reglasnotificacion
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReglaNotificacionDto>>> Listar()
    {
        return Ok(await _reglaService.ListarAsync());
    }

    // GET: api/reglasnotificacion/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReglaNotificacionDto>> Obtener(Guid id)
    {
        var regla = await _reglaService.ObtenerAsync(id);

        return regla is null
            ? NotFound(new { mensaje = "La regla no existe." })
            : Ok(regla);
    }

    // POST: api/reglasnotificacion
    [HttpPost]
    public async Task<ActionResult<ReglaNotificacionDto>> Crear(
        [FromBody] CrearReglaDto dto)
    {
        try
        {
            var regla = await _reglaService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(Obtener),
                new { id = regla.Id },
                regla);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // PUT: api/reglasnotificacion/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ReglaNotificacionDto>> Actualizar(
        Guid id,
        [FromBody] ActualizarReglaDto dto)
    {
        try
        {
            var regla = await _reglaService.ActualizarAsync(id, dto);

            return regla is null
                ? NotFound(new { mensaje = "La regla no existe." })
                : Ok(regla);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // PATCH: api/reglasnotificacion/{id}/estado
    [HttpPatch("{id:guid}/estado")]
    public async Task<ActionResult<ReglaNotificacionDto>> CambiarEstado(
        Guid id,
        [FromBody] EstadoDto dto)
    {
        var regla = await _reglaService.CambiarEstadoAsync(id, dto.Activa);

        return regla is null
            ? NotFound(new { mensaje = "La regla no existe." })
            : Ok(regla);
    }

    // DELETE: api/reglasnotificacion/{id}
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Eliminar(Guid id)
    {
        return await _reglaService.EliminarAsync(id)
            ? NoContent()
            : NotFound(new { mensaje = "La regla no existe." });
    }

    public class EstadoDto
    {
        public bool Activa { get; set; }
    }

    public class ProbarDto
    {
        public Guid? ClienteId { get; set; }

        public Guid? PrestamoId { get; set; }
    }

    // POST: api/reglasnotificacion/{id}/probar
    [HttpPost("{id:guid}/probar")]
    public async Task<ActionResult<object>> Probar(
        Guid id,
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

        var (exito, mensaje) = await _reglaService.ProbarAsync(
            id,
            usuarioId,
            dto.ClienteId,
            dto.PrestamoId);

        return Ok(new { exito, mensaje });
    }

    // GET: api/reglasnotificacion/envios?top=50
    [HttpGet("envios")]
    public async Task<ActionResult<IEnumerable<EnvioNotificacionDto>>> Envios(
        [FromQuery] int top = 50)
    {
        return Ok(await _reglaService.ObtenerEnviosAsync(top));
    }

    // GET: api/reglasnotificacion/diagnostico
    [HttpGet("diagnostico")]
    public async Task<ActionResult<object>> Diagnostico(
        [FromServices] IProgramadorService programador)
    {
        var ahoraUtc = DateTime.UtcNow;
        var lima = ProgramadorService.AhoraLima(ahoraUtc);

        var reglas = (await _reglaService.ListarAsync()).ToList();

        var tocan = reglas
            .Where(r =>
                r.Activa &&
                ProgramadorService.TocaHoy(
                    new() { DiasSemana = r.DiasSemana }, lima) &&
                r.Hora == lima.ToString("HH:mm"))
            .Select(r => r.Nombre)
            .ToList();

        return Ok(new
        {
            horaUtc = ahoraUtc.ToString("HH:mm:ss"),
            horaLima = lima.ToString("HH:mm:ss"),
            ultimoTickUtc = ProgramadorJob.UltimoTickUtc?.ToString("HH:mm:ss"),
            reglasActivas = reglas.Count(r => r.Activa),
            tocanAhora = tocan
        });
    }
}
