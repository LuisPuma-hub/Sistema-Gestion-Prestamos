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

    // DELETE: api/clientes/{id} (solo ADMIN, sin préstamos)
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        try
        {
            var eliminado = await _clienteService.EliminarAsync(id);

            if (!eliminado)
            {
                return NotFound(new
                {
                    mensaje = "Cliente no encontrado."
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

    // GET: api/clientes/{id}/foto
    [HttpGet("{id:guid}/foto")]
    public async Task<IActionResult> ObtenerFoto(Guid id)
    {
        var cliente = await _clienteService.ObtenerPorIdAsync(id);

        if (cliente?.FotoReciboServicio is null)
        {
            return NotFound();
        }

        var rutaFisica = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            cliente.FotoReciboServicio.Replace('/', Path.DirectorySeparatorChar));

        if (!System.IO.File.Exists(rutaFisica))
        {
            return NotFound();
        }

        var extension = Path.GetExtension(rutaFisica).ToLowerInvariant();

        var contentType = extension == ".png" ? "image/png" : "image/jpeg";

        return PhysicalFile(rutaFisica, contentType);
    }

    // POST: api/clientes/{id}/foto (jpg/png <= 5MB)
    [HttpPost("{id:guid}/foto")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<ActionResult<object>> SubirFoto(
        Guid id,
        IFormFile archivo)
    {
        var cliente = await _clienteService.ObtenerPorIdAsync(id);

        if (cliente is null)
        {
            return NotFound(new
            {
                mensaje = "Cliente no encontrado."
            });
        }

        if (archivo is null || archivo.Length == 0)
        {
            return BadRequest(new
            {
                mensaje = "Adjunte una imagen."
            });
        }

        if (archivo.Length > 5 * 1024 * 1024)
        {
            return BadRequest(new
            {
                mensaje = "La imagen no debe superar 5 MB."
            });
        }

        var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();

        if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
        {
            return BadRequest(new
            {
                mensaje = "Solo se permiten imágenes JPG o PNG."
            });
        }

        var carpeta = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot", "recibos");

        Directory.CreateDirectory(carpeta);

        var nombre = $"{id:N}_{Guid.NewGuid():N}{extension}";
        var rutaFisica = Path.Combine(carpeta, nombre);

        await using (var flujo = System.IO.File.Create(rutaFisica))
        {
            await archivo.CopyToAsync(flujo);
        }

        var relativo = $"recibos/{nombre}";

        await _clienteService.ActualizarFotoAsync(id, relativo);

        return Ok(new
        {
            mensaje = "Foto guardada.",
            ruta = relativo
        });
    }

    // DELETE: api/clientes/{id}/cascada (solo ADMIN, borra todo)
    [HttpDelete("{id:guid}/cascada")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ResumenCascada>> EliminarCascada(Guid id)
    {
        try
        {
            var resumen = await _clienteService.EliminarCascadaAsync(id);

            return Ok(resumen);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new
            {
                mensaje = ex.Message
            });
        }
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