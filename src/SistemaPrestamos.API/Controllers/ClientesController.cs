using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    // GET: api/clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> ObtenerTodos()
    {
        var clientes = await _clienteService.ObtenerTodosAsync();

        return Ok(clientes);
    }

    // GET: api/clientes/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClienteDto>> ObtenerPorId(Guid id)
    {
        var cliente = await _clienteService.ObtenerPorIdAsync(id);

        if (cliente is null)
        {
            return NotFound(new
            {
                mensaje = "Cliente no encontrado."
            });
        }

        return Ok(cliente);
    }

    // GET: api/clientes/documento/{numeroDocumento}
    [HttpGet("documento/{numeroDocumento}")]
    public async Task<ActionResult<ClienteDto>> ObtenerPorDocumento(
        string numeroDocumento)
    {
        var cliente = await _clienteService
            .ObtenerPorDocumentoAsync(numeroDocumento);

        if (cliente is null)
        {
            return NotFound(new
            {
                mensaje = "Cliente no encontrado."
            });
        }

        return Ok(cliente);
    }

    // POST: api/clientes
    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Crear(
        [FromBody] CrearClienteDto dto)
    {
        try
        {
            var cliente = await _clienteService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = cliente.Id },
                cliente);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                mensaje = ex.Message
            });
        }
    }

    // PUT: api/clientes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] CrearClienteDto dto)
    {
        var actualizado = await _clienteService
            .ActualizarAsync(id, dto);

        if (!actualizado)
        {
            return NotFound(new
            {
                mensaje = "Cliente no encontrado."
            });
        }

        return NoContent();
    }

    // PATCH: api/clientes/{id}/estado
    [HttpPatch("{id:guid}/estado")]
public async Task<IActionResult> CambiarEstado(
    Guid id,
    [FromBody] CambiarEstadoClienteDto dto)
{
    var estadosPermitidos = new[]
    {
        "Activo",
        "En observación",
        "Moroso"
    };

    if (!estadosPermitidos.Contains(dto.Estado))
    {
        return BadRequest(new
        {
            mensaje = "Estado no válido.",
            estadosPermitidos
        });
    }

    var actualizado = await _clienteService
        .CambiarEstadoAsync(id, dto.Estado);

    if (!actualizado)
    {
        return NotFound(new
        {
            mensaje = "Cliente no encontrado."
        });
    }

    return NoContent();
}
}

public class CambiarEstadoClienteDto
{
    public string Estado { get; set; } = string.Empty;
}