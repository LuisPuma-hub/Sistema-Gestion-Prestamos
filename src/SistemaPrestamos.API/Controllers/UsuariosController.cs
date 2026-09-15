using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerTodos()
    {
        var usuarios = await _usuarioService.ObtenerTodosAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UsuarioDto>> ObtenerPorId(
        Guid id)
    {
        var usuario = await _usuarioService.ObtenerPorIdAsync(id);

        if (usuario is null)
        {
            return NotFound(new
            {
                mensaje = "Usuario no encontrado."
            });
        }

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Crear(
        [FromBody] CrearUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = usuario.Id },
                usuario);
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