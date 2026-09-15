using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MorosidadesController : ControllerBase
{
    private readonly IMorosidadService _morosidadService;

    public MorosidadesController(
        IMorosidadService morosidadService)
    {
        _morosidadService = morosidadService;
    }

    [HttpGet("prestamo/{prestamoId:guid}")]
    public async Task<ActionResult<MorosidadDto>> ObtenerPorPrestamo(
        Guid prestamoId)
    {
        try
        {
            var morosidad = await _morosidadService
                .ObtenerPorPrestamoAsync(prestamoId);

            if (morosidad is null)
            {
                return NotFound(new
                {
                    mensaje = "El préstamo no tiene registro de morosidad."
                });
            }

            return Ok(morosidad);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPost("prestamo/{prestamoId:guid}/evaluar")]
    public async Task<ActionResult<MorosidadDto>> Evaluar(
        Guid prestamoId,
        [FromQuery] DateTime? fechaReferencia)
    {
        try
        {
            var fecha = fechaReferencia ?? DateTime.UtcNow;

            var morosidad = await _morosidadService
                .EvaluarAsync(
                    prestamoId,
                    fecha);

            return Ok(morosidad);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPatch("prestamo/{prestamoId:guid}/reactivar")]
    public async Task<ActionResult<MorosidadDto>> Reactivar(
        Guid prestamoId,
        [FromBody] ReactivarMorosidadDto dto)
    {
        try
        {
            var morosidad = await _morosidadService
                .ReactivarAsync(
                    prestamoId,
                    dto.Observaciones);

            return Ok(morosidad);
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
