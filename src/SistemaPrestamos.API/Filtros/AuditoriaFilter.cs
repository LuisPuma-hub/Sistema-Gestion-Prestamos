using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.API.Filtros;

/// <summary>
/// Registra operaciones de escritura (POST/PUT/PATCH/DELETE).
/// Nunca guarda cuerpos (pueden traer claves).
/// Solo metadatos: quién, qué, cuándo y resultado.
/// </summary>
public class AuditoriaFilter : IAsyncActionFilter
{
    private readonly PrestamosDbContext _context;

    public AuditoriaFilter(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var metodo = context.HttpContext.Request.Method;

        if (metodo == HttpMethods.Get || metodo == HttpMethods.Head)
        {
            await next();
            return;
        }

        var ejecutado = await next();

        try
        {
            var idUsuario = context.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            var ruta = context.HttpContext.Request.Path.ToString();

            var registro = "-";

            foreach (var clave in new[] { "id", "clienteId", "prestamoId" })
            {
                if (context.ActionArguments.TryGetValue(clave, out var valor)
                    && valor is not null)
                {
                    registro = valor.ToString() ?? "-";
                    break;
                }
            }

            var estado = ejecutado.Result switch
            {
                Microsoft.AspNetCore.Mvc.ObjectResult o => o.StatusCode,
                Microsoft.AspNetCore.Mvc.StatusCodeResult s => s.StatusCode,
                _ => null
            };

            await _context.Auditorias.AddAsync(new Auditoria
            {
                Id = Guid.NewGuid(),
                Tabla = context.Controller.GetType().Name
                    .Replace("Controller", string.Empty),
                IdRegistro = registro,
                Operacion = metodo,
                Diff = JsonSerializer.Serialize(new
                {
                    ruta,
                    estado
                }),
                UsuarioId = Guid.TryParse(idUsuario, out var uid)
                    ? uid
                    : null,
                Fecha = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }
        catch
        {
            // Auditar nunca debe tumbar la operación real.
        }
    }
}
