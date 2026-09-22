using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WhatsappController : ControllerBase
{
    private readonly IWhatsappService _whatsappService;

    public WhatsappController(IWhatsappService whatsappService)
    {
        _whatsappService = whatsappService;
    }

    public class ProbarDto
    {
        public string Numero { get; set; } = string.Empty;
        public string? Texto { get; set; }
        public string? Plantilla { get; set; }
        public string Idioma { get; set; } = "es_PE";
        public List<string> Parametros { get; set; } = new();
    }

    public class RecordatorioDto
    {
        public Guid ClienteId { get; set; }
        public Guid? PrestamoId { get; set; }
    }

    public class EnviarPlantillaDto
    {
        public Guid ClienteId { get; set; }
        public Guid? PrestamoId { get; set; }
        public string Plantilla { get; set; } = string.Empty;
    }

    // POST: api/whatsapp/probar
    [HttpPost("probar")]
    public async Task<ActionResult<object>> Probar(
        [FromBody] ProbarDto dto)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(dto.Plantilla))
            {
                var tpl = await _whatsappService.EnviarPlantillaAsync(
                    null,
                    null,
                    dto.Numero,
                    dto.Plantilla,
                    $"Plantilla {dto.Plantilla}.",
                    string.IsNullOrWhiteSpace(dto.Idioma) ? "es_PE" : dto.Idioma,
                    dto.Parametros);

                return Ok(new
                {
                    mensaje = "Plantilla enviada.",
                    id = tpl.Id,
                    idExterno = tpl.IdentificadorExterno
                });
            }

            if (!string.IsNullOrWhiteSpace(dto.Texto))
            {
                var texto = await _whatsappService.EnviarTextoAsync(
                    null,
                    null,
                    dto.Numero,
                    dto.Texto);

                return Ok(new
                {
                    mensaje = "Texto libre enviado.",
                    id = texto.Id,
                    idExterno = texto.IdentificadorExterno
                });
            }

            var mensaje = await _whatsappService.EnviarPlantillaAsync(
                null,
                null,
                dto.Numero,
                "hello_world",
                "Plantilla de prueba hello_world.",
                "en_US");

            return Ok(new
            {
                mensaje = "WhatsApp de prueba enviado.",
                id = mensaje.Id,
                idExterno = mensaje.IdentificadorExterno
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // POST: api/whatsapp/recordatorio
    [HttpPost("recordatorio")]
    public async Task<ActionResult<MensajeWhatsapp>> Recordatorio(
        [FromBody] RecordatorioDto dto)
    {
        try
        {
            var mensaje = await _whatsappService.EnviarRecordatorioAsync(
                dto.ClienteId,
                dto.PrestamoId);

            return Ok(mensaje);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // GET: api/whatsapp/plantillas
    [HttpGet("plantillas")]
    public ActionResult<object> Plantillas()
    {
        return Ok(SistemaPrestamos.Application.Services.WhatsappService
            .PlantillasDisponibles
            .Select(p => new
            {
                nombre = p.Nombre,
                descripcion = p.Descripcion
            }));
    }

    // POST: api/whatsapp/enviar-plantilla
    [HttpPost("enviar-plantilla")]
    public async Task<ActionResult<MensajeWhatsapp>> EnviarPlantilla(
        [FromBody] EnviarPlantillaDto dto)
    {
        try
        {
            var mensaje = await _whatsappService.EnviarPlantillaCatalogoAsync(
                dto.ClienteId,
                dto.PrestamoId,
                dto.Plantilla);

            return Ok(mensaje);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // GET: api/whatsapp/historial/cliente/{clienteId}
    [HttpGet("historial/cliente/{clienteId:guid}")]
    public async Task<ActionResult<IEnumerable<MensajeWhatsapp>>> Historial(
        Guid clienteId)
    {
        var historial = await _whatsappService
            .ObtenerHistorialAsync(clienteId);

        return Ok(historial);
    }
}
