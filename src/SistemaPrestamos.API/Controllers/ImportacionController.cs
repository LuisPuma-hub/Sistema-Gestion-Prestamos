using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class ImportacionController : ControllerBase
{
    private const long TamanoMaximoBytes = 2 * 1024 * 1024;

    private readonly IImportacionService _importacionService;

    public ImportacionController(IImportacionService importacionService)
    {
        _importacionService = importacionService;
    }

    // GET: api/importacion/prestamos/plantilla
    [HttpGet("prestamos/plantilla")]
    public IActionResult DescargarPlantilla()
    {
        var csv =
            "idFila,tipoDoc,nroDoc,nombres,apellidos,telefono," +
            "direccion,capitalInicial,fechaInicio,saldoCapitalActual\n" +
            "1,DNI,70000001,Juan,Perez,987654321,Av. Lima 123," +
            "500.00,2026-08-01,400.00\n" +
            "2,DNI,70000002,Maria,Lopez,912345678,Jr. Rosas 45," +
            "300.00,2026-09-01,\n";

        return File(
            Encoding.UTF8.GetBytes(csv),
            "text/csv",
            "plantilla_prestamos.csv");
    }

    // POST: api/importacion/prestamos/preview
    [HttpPost("prestamos/preview")]
    [RequestSizeLimit(TamanoMaximoBytes)]
    public async Task<ActionResult<ImportacionPreviewDto>> Previsualizar(
        IFormFile archivo,
        [FromQuery] DateTime? fechaCorte)
    {
        try
        {
            var contenido = await LeerCsvAsync(archivo);

            var resultado = await _importacionService
                .PrevisualizarAsync(contenido, fechaCorte);

            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // POST: api/importacion/prestamos/confirm
    [HttpPost("prestamos/confirm")]
    [RequestSizeLimit(TamanoMaximoBytes)]
    public async Task<ActionResult<ImportacionResultadoDto>> Confirmar(
        IFormFile archivo,
        [FromQuery] DateTime? fechaCorte)
    {
        try
        {
            var contenido = await LeerCsvAsync(archivo);

            var resultado = await _importacionService
                .ConfirmarAsync(contenido, fechaCorte);

            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    private static async Task<string> LeerCsvAsync(IFormFile archivo)
    {
        if (archivo is null || archivo.Length == 0)
            throw new InvalidOperationException(
                "Adjunte el archivo CSV.");

        if (archivo.Length > TamanoMaximoBytes)
            throw new InvalidOperationException(
                "El archivo supera 2 MB.");

        using var lector = new StreamReader(
            archivo.OpenReadStream(),
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: true);

        var contenido = await lector.ReadToEndAsync();

        // Tolerar BOM.
        return contenido.TrimStart('\uFEFF');
    }
}
