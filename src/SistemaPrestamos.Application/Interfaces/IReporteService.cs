using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IReporteService
{
    Task<CobranzaDto> CobranzaDelDiaAsync(DateTime? fecha = null);

    Task<CarteraDto> CarteraAsync();

    Task<List<IngresoDiaDto>> IngresosAsync(DateTime desde, DateTime hasta);
}
