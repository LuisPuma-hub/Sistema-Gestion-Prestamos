using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.Application.Services;

public class ReporteService : IReporteService
{
    private readonly IPagoRepository _pagoRepository;
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IPeriodoInteresRepository _periodoRepository;
    private readonly IFondoRepository _fondoRepository;

    public ReporteService(
        IPagoRepository pagoRepository,
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoRepository,
        IFondoRepository? fondoRepository = null)
    {
        _pagoRepository = pagoRepository;
        _prestamoRepository = prestamoRepository;
        _periodoRepository = periodoRepository;
        _fondoRepository = fondoRepository!;
    }

    public async Task<CobranzaDto> CobranzaDelDiaAsync(
        DateTime? fecha = null)
    {
        var dia = fecha?.Date ?? DateTime.UtcNow.Date;

        var pagos = await _pagoRepository.ObtenerTodosAsync();

        var delDia = pagos
            .Where(x =>
                EsCobro(x.Estado) &&
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
                EsCobro(x.Estado) &&
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

    public async Task<CapitalDto> CapitalAsync()
    {
        if (_fondoRepository is null)
            throw new InvalidOperationException(
                "Fondo no disponible.");

        var movimientos = await _fondoRepository.ObtenerTodosAsync();

        var aportes = movimientos
            .Where(x => EsAporte(x.Tipo))
            .Sum(x => x.Monto);

        var retiros = movimientos
            .Where(x => !EsAporte(x.Tipo))
            .Sum(x => x.Monto);

        var prestamos = await _prestamoRepository.ObtenerTodosAsync();

        var colocado = prestamos
            .Where(EsVigente)
            .Sum(x => x.CapitalPendiente);

        var pagos = await _pagoRepository.ObtenerTodosAsync();

        var vigentes = pagos
            .Where(x => EsCobro(x.Estado))
            .ToList();

        return new CapitalDto
        {
            Aportes = aportes,
            Retiros = retiros,
            Colocado = colocado,
            GanadoIntereses = vigentes.Sum(x => x.MontoInteres),
            CapitalRecuperado = vigentes.Sum(x => x.MontoCapital)
        };
    }

    public async Task<List<FondoMovimientoDto>> MovimientosAsync()
    {
        if (_fondoRepository is null)
            throw new InvalidOperationException(
                "Fondo no disponible.");

        var movimientos = await _fondoRepository.ObtenerTodosAsync();

        return movimientos.Select(MapearMovimiento).ToList();
    }

    public async Task<FondoMovimientoDto> RegistrarMovimientoAsync(
        CrearFondoMovimientoDto dto)
    {
        if (_fondoRepository is null)
            throw new InvalidOperationException(
                "Fondo no disponible.");

        var tipo = dto.Tipo?.Trim();

        if (!string.Equals(tipo, "Aporte",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(tipo, "Retiro",
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "El tipo debe ser Aporte o Retiro.");

        if (dto.Monto <= 0)
            throw new InvalidOperationException(
                "El monto debe ser mayor que cero.");

        if (!string.IsNullOrWhiteSpace(dto.Motivo) &&
            dto.Motivo.Trim().Length > 200)
            throw new InvalidOperationException(
                "El motivo supera 200 caracteres.");

        var fecha = dto.Fecha == default
            ? DateTime.UtcNow
            : DateTime.SpecifyKind(dto.Fecha, DateTimeKind.Utc);

        if (fecha.Date > DateTime.UtcNow.Date)
            throw new InvalidOperationException(
                "La fecha no puede ser futura.");

        var movimiento = new Domain.Entities.FondoMovimiento
        {
            Id = Guid.NewGuid(),
            Tipo = string.Equals(tipo, "Aporte",
                StringComparison.OrdinalIgnoreCase)
                    ? "Aporte"
                    : "Retiro",
            Monto = Math.Round(dto.Monto, 2),
            Fecha = fecha,
            Motivo = string.IsNullOrWhiteSpace(dto.Motivo)
                ? null
                : dto.Motivo.Trim(),
            FechaRegistro = DateTime.UtcNow
        };

        await _fondoRepository.CrearAsync(movimiento);
        await _fondoRepository.GuardarCambiosAsync();

        return MapearMovimiento(movimiento);
    }

    public async Task<bool> EliminarMovimientoAsync(Guid id)
    {
        if (_fondoRepository is null)
            throw new InvalidOperationException(
                "Fondo no disponible.");

        var movimiento = await _fondoRepository.ObtenerPorIdAsync(id);

        if (movimiento is null)
            return false;

        await _fondoRepository.EliminarAsync(movimiento);
        await _fondoRepository.GuardarCambiosAsync();

        return true;
    }

    private static bool EsVigente(
        Domain.Entities.Prestamo prestamo)
    {
        return string.Equals(
                prestamo.Estado, "Activo",
                StringComparison.OrdinalIgnoreCase) ||
            string.Equals(
                prestamo.Estado, "Moroso",
                StringComparison.OrdinalIgnoreCase);
    }

    private static FondoMovimientoDto MapearMovimiento(
        Domain.Entities.FondoMovimiento movimiento)
    {
        return new FondoMovimientoDto
        {
            Id = movimiento.Id,
            Tipo = movimiento.Tipo,
            Monto = movimiento.Monto,
            Fecha = movimiento.Fecha,
            Motivo = movimiento.Motivo
        };
    }

    private static bool EsAnulado(string estado)
    {
        return string.Equals(
            estado, "Anulado", StringComparison.OrdinalIgnoreCase);
    }

    private static bool EsCobro(string estado)
    {
        // Solo Registrado mueve dinero real. Anulado y Ajuste
        // se excluyen de cobranza, ingresos y recuperado.
        return string.Equals(
            estado, "Registrado", StringComparison.OrdinalIgnoreCase);
    }

    private static bool EsAporte(string tipo)
    {
        return string.Equals(
            tipo, "Aporte", StringComparison.OrdinalIgnoreCase);
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
