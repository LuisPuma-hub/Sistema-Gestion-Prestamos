using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.Application.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _auditoriaRepository;

    public AuditoriaService(IAuditoriaRepository auditoriaRepository)
    {
        _auditoriaRepository = auditoriaRepository;
    }

    public async Task<IEnumerable<AuditoriaDto>> ObtenerRecientesAsync(int top)
    {
        if (top is < 1 or > 500)
        {
            top = 50;
        }

        var registros = await _auditoriaRepository.ObtenerRecientesAsync(top);

        return registros.Select(x => new AuditoriaDto
        {
            Id = x.Id,
            Tabla = x.Tabla,
            IdRegistro = x.IdRegistro,
            Operacion = x.Operacion,
            Diff = x.Diff,
            UsuarioId = x.UsuarioId,
            Fecha = x.Fecha
        });
    }
}
