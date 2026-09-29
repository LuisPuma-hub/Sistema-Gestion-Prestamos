using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.Application.Services;

public class ReporteService : IReporteService
{
    private readonly IPagoRepository _pagoRepository;
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IPeriodoInteresRepository _periodoRepository;

    public ReporteService(
        IPagoRepository pagoRepository,
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoRepository)
    {
        _pagoRepository = pagoRepository;
        _prestamoRepository = prestamoRepository;
        _periodoRepository = periodoRepository;
    }

    public async Task<CobranzaDto> CobranzaDelDiaAsync(
        DateTime? fecha = null)
    {
        var dia = fecha?.Date ?? DateTime.UtcNow.Date;

        var pagos = await _pagoRepository.ObtenerTodosAsync();

        var delDia = pagos
            .Where(x =>
                !EsAnulado(x.Estado) &&
                x.FechaPago.Date == dia)
            .ToList();

        return new CobranzaDto
        {
            Fecha = DateTime.SpecifyKind(dia, DateTimeKind.Utc),
            Total = delDia.Sum(x => x.Monto),
            AInteres = delDia.Sum(x => x.MontoInteres),
            ACapital = delDia.Sum(x => x.MontoCapital),
            NumPagos = delDia.Count
        };
    }

    public async Task<CarteraDto> CarteraAsync()
    {
        var prestamos = await _prestamoRepository.ObtenerTodosAsync();

        var vigentes = prestamos
            .Where(x =>
                string.Equals(
                    x.Estado, "Activo",
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(
                    x.Estado, "Moroso",
                    StringComparison.OrdinalIgnoreCase))
            .OrderBy(NombreCliente)
            .ToList();

        var filas = new List<CarteraFilaDto>();

        foreach (var prestamo in vigentes)
        {
            var periodos = await _periodoRepository
                .ObtenerPorPrestamoAsync(prestamo.Id);

            filas.Add(new CarteraFilaDto
            {
                PrestamoId = prestamo.Id,
                Cliente = NombreCliente(prestamo),
                CapitalPendiente = prestamo.CapitalPendiente,
                InteresesPendientes = periodos.Sum(
                    x => x.InteresPendiente),
                Estado = prestamo.Estado
            });
        }

        return new CarteraDto
        {
            CapitalTotal = filas.Sum(x => x.CapitalPendiente),
            InteresesTotal = filas.Sum(x => x.InteresesPendientes),
            NumPrestamos = filas.Count,
            Filas = filas
        };
    }

    public async Task<List<IngresoDiaDto>> IngresosAsync(
        DateTime desde,
        DateTime hasta)
    {
        var inicio = desde.Date;
        var fin = hasta.Date;

        if (fin < inicio)
            throw new InvalidOperationException(
                "El rango de fechas es inválido.");

        if ((fin - inicio).TotalDays > 366)
            throw new InvalidOperationException(
                "El rango máximo es de un año.");

        var pagos = await _pagoRepository.ObtenerTodosAsync();

        return pagos
            .Where(x =>
                !EsAnulado(x.Estado) &&
                x.FechaPago.Date >= inicio &&
                x.FechaPago.Date <= fin)
            .GroupBy(x => x.FechaPago.Date)
            .OrderBy(g => g.Key)
            .Select(g => new IngresoDiaDto
            {
                Fecha = DateTime.SpecifyKind(g.Key, DateTimeKind.Utc),
                Intereses = g.Sum(x => x.MontoInteres),
                Capital = g.Sum(x => x.MontoCapital),
                NumPagos = g.Count()
            })
            .ToList();
    }

    private static bool EsAnulado(string estado)
    {
        return string.Equals(
            estado, "Anulado", StringComparison.OrdinalIgnoreCase);
    }

    private static string NombreCliente(
        Domain.Entities.Prestamo prestamo)
    {
        if (prestamo.Cliente is null)
            return "-";

        return $"{prestamo.Cliente.Nombres} " +
               $"{prestamo.Cliente.Apellidos}".Trim();
    }
}
