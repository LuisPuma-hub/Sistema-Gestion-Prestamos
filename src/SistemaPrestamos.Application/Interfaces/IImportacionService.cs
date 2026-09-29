using SistemaPrestamos.Application.DTOs;

namespace SistemaPrestamos.Application.Interfaces;

public interface IImportacionService
{
    Task<ImportacionPreviewDto> PrevisualizarAsync(
        string contenidoCsv,
        DateTime? fechaCorte = null);

    Task<ImportacionResultadoDto> ConfirmarAsync(
        string contenidoCsv,
        DateTime? fechaCorte = null);
}
