using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class EsquemaSaldoTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;
    private readonly ClienteService _clientes;
    private readonly PrestamoService _prestamos;
    private readonly PagoService _pagos;
    private readonly PeriodoInteresService _periodos;

    public EsquemaSaldoTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"esq_{Guid.NewGuid()}")
            .Options;

        _contexto = new PrestamosDbContext(opciones);

        var repoCliente = new ClienteRepository(_contexto);
        var repoPrestamo = new PrestamoRepository(_contexto);
        var repoPago = new PagoRepository(_contexto);
        var repoPeriodo = new PeriodoInteresRepository(_contexto);
        var repoMorosidad = new MorosidadRepository(_contexto);

        var httpContext = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext()
        };

        _clientes = new ClienteService(
            repoCliente,
            repoPrestamo,
            repoPago,
            repoPeriodo,
            repoMorosidad,
            new MensajeWhatsappRepository(_contexto),
            new GaranteRepository(_contexto),
            httpContext);

        _prestamos = new PrestamoService(
            repoPrestamo,
            repoCliente,
            repoPeriodo,
            repoPago);

        _periodos = new PeriodoInteresService(
            repoPrestamo,
            repoPeriodo);

        _pagos = new PagoService(
            repoPago,
            repoPrestamo,
            repoPeriodo,
            _periodos);
    }

    public void Dispose()
    {
        _contexto.Dispose();
    }

    private async Task<Guid> CrearClienteAsync(string documento)
    {
        var dto = await _clientes.CrearAsync(new CrearClienteDto
        {
            TipoDocumento = "DNI",
            NumeroDocumento = documento,
            Nombres = "Test",
            Apellidos = "Esq",
            Telefono = "987654321"
        });

        return dto.Id;
    }

    [Fact]
    public async Task Crear_DefectoEsSaldo()
    {
        var cliente = await CrearClienteAsync("81000001");

        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 500m,
            FechaInicio = DateTime.UtcNow
        });

        Assert.Equal("Saldo", creado.EsquemaInteres);
    }

    [Fact]
    public async Task Crear_EsquemaInvalido_SeRechaza()
    {
        var cliente = await CrearClienteAsync("81000002");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _prestamos.CrearAsync(new CrearPrestamoDto
            {
                ClienteId = cliente,
                CapitalInicial = 500m,
                FechaInicio = DateTime.UtcNow,
                EsquemaInteres = "Mixto"
            }));
    }

    [Fact]
    public async Task Saldo_NuevaSemanaUsaPendiente()
    {
        var cliente = await CrearClienteAsync("81000003");

        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 1000m,
            FechaInicio = DateTime.UtcNow.AddDays(-14),
            EsquemaInteres = "Saldo"
        });

        Assert.Equal("Saldo", creado.EsquemaInteres);
        await _prestamos.AprobarAsync(creado.Id);

        // Primera semana sin pagos: igual que Fijo.
        var inicial = _contexto.PeriodosInteres
            .Where(x => x.PrestamoId == creado.Id)
            .ToList();
        Assert.All(inicial, x => Assert.Equal(50m, x.InteresGenerado));

        // Pago que baja el pendiente a 600.
        await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = creado.Id,
            Monto = 550m,
            FechaPago = DateTime.UtcNow
        });

        // Semanas futuras usan el pendiente (600 x 5% = 30.00).
        await _periodos.GenerarPeriodosPendientesAsync(
            creado.Id, DateTime.UtcNow.AddDays(21));

        var todos = _contexto.PeriodosInteres
            .Where(x => x.PrestamoId == creado.Id)
            .OrderBy(x => x.FechaInicio)
            .ToList();

        Assert.Contains(todos, x => x.InteresGenerado == 30.00m);
    }

    [Fact]
    public async Task Interes_RedondeaHalfUp()
    {
        var cliente = await CrearClienteAsync("81000004");

        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 99.99m,
            FechaInicio = DateTime.UtcNow,
            EsquemaInteres = "Saldo"
        });

        await _prestamos.AprobarAsync(creado.Id);

        var periodo = _contexto.PeriodosInteres
            .Single(x => x.PrestamoId == creado.Id);

        Assert.Equal(5.00m, periodo.InteresGenerado);
    }

    [Fact]
    public async Task Diaria_Plan_500_28()
    {
        var cliente = await CrearClienteAsync("82000001");

        // Lunes 05/10/2026 12:00 UTC = lunes 07:00 Lima.
        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 500m,
            FechaInicio = new DateTime(2026, 10, 5, 12, 0, 0),
            EsquemaInteres = "Saldo",
            ModalidadCobro = "PlazoFijo",
            PlazoValor = 28,
            PlazoUnidad = "Dias"
        });

        await _prestamos.AprobarAsync(creado.Id);

        var prestamo = await _prestamos.ObtenerPorIdAsync(creado.Id);

        Assert.NotNull(prestamo);
        Assert.Equal("PlazoFijo", prestamo!.ModalidadCobro);
        Assert.Equal(28, prestamo.PlazoCuotas);
        Assert.Equal(600m, prestamo.MontoTotal);
        Assert.Equal(21.50m, prestamo.CuotaFija);
        Assert.NotNull(prestamo.FechaFin);

        var cuotas = _contexto.PeriodosInteres
            .Where(x => x.PrestamoId == creado.Id)
            .OrderBy(x => x.FechaInicio)
            .ToList();

        Assert.Equal(28, cuotas.Count);
        Assert.All(cuotas, c => Assert.True(c.EsCuotaFija));
        Assert.Equal(100m, cuotas.Sum(x => x.InteresGenerado));

        // Sin cuotas en domingo (hora Lima).
        Assert.DoesNotContain(cuotas, c =>
            PeriodoInteresService.EsDomingoLima(c.FechaInicio));
        Assert.DoesNotContain(cuotas, c =>
            PeriodoInteresService.EsDomingoLima(c.FechaVencimiento));

        Assert.Equal(
            cuotas.Last().FechaVencimiento, prestamo.FechaFin);
    }

    [Fact]
    public async Task PlazoFijo_Semanal_4Cuotas()
    {
        var cliente = await CrearClienteAsync("82000005");

        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 500m,
            FechaInicio = new DateTime(2026, 10, 5, 12, 0, 0),
            ModalidadCobro = "PlazoFijo",
            FrecuenciaCuota = "Semanal",
            PlazoValor = 4,
            PlazoUnidad = "Semanas"
        });

        await _prestamos.AprobarAsync(creado.Id);

        var prestamo = await _prestamos.ObtenerPorIdAsync(creado.Id);

        Assert.NotNull(prestamo);
        Assert.Equal(4, prestamo!.PlazoCuotas);
        Assert.Equal("Semanal", prestamo.FrecuenciaCuota);
        Assert.Equal(600m, prestamo.MontoTotal);
        Assert.Equal(150.00m, prestamo.CuotaFija);

        var cuotas = _contexto.PeriodosInteres
            .Where(x => x.PrestamoId == creado.Id)
            .OrderBy(x => x.FechaInicio)
            .ToList();

        Assert.Equal(4, cuotas.Count);
        Assert.All(cuotas, c => Assert.True(c.EsCuotaFija));
        Assert.Equal(100m, cuotas.Sum(x => x.InteresGenerado));
    }

    [Fact]
    public void Diaria_Cuota_RedondeaHaciaArriba()
    {
        var (_, _, cuota) =
            PeriodoInteresService.CalcularPlanDiario(100m, 0.05m, 28);

        // 120/28 = 4.2857 -> 4.30.
        Assert.Equal(4.30m, cuota);

        var (_, total, cuota2) =
            PeriodoInteresService.CalcularPlanDiario(500m, 0.05m, 28);

        Assert.Equal(600m, total);
        Assert.Equal(21.50m, cuota2);
    }

    [Fact]
    public async Task Diaria_Rechaza_SinPlazo()
    {
        var cliente = await CrearClienteAsync("82000002");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _prestamos.CrearAsync(new CrearPrestamoDto
            {
                ClienteId = cliente,
                CapitalInicial = 500m,
                FechaInicio = DateTime.UtcNow,
                ModalidadCobro = "PlazoFijo"
            }));
    }

    [Fact]
    public async Task Diaria_Domingo_NoGeneraMoraNueva()
    {
        var cliente = await CrearClienteAsync("82000003");

        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 300m,
            FechaInicio = new DateTime(2026, 10, 5, 12, 0, 0),
            ModalidadCobro = "PlazoFijo",
            PlazoValor = 6,
            PlazoUnidad = "Dias"
        });

        await _prestamos.AprobarAsync(creado.Id);

        var moras = new MorosidadService(
            new PrestamoRepository(_contexto),
            new PeriodoInteresRepository(_contexto),
            new MorosidadRepository(_contexto),
            _periodos);

        // Sábado 10/10 12:00 Lima y domingo 11/10 12:00 Lima.
        var sabado = new DateTime(2026, 10, 10, 17, 0, 0);
        var domingo = new DateTime(2026, 10, 11, 17, 0, 0);

        var evalSabado = await moras.EvaluarAsync(creado.Id, sabado);
        var evalDomingo = await moras.EvaluarAsync(creado.Id, domingo);

        Assert.Equal(
            evalSabado.PagosInteresVencidos,
            evalDomingo.PagosInteresVencidos);
    }

    [Fact]
    public async Task Diaria_VencidoPlazo_RetornaSemanal()
    {
        var cliente = await CrearClienteAsync("82000004");

        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 300m,
            FechaInicio = new DateTime(2026, 10, 5, 12, 0, 0),
            ModalidadCobro = "PlazoFijo",
            PlazoValor = 6,
            PlazoUnidad = "Dias"
        });

        await _prestamos.AprobarAsync(creado.Id);

        // Muy pasado el fin (6 cuotas Lun-Sáb terminan ~13/10).
        await _periodos.GenerarPeriodosPendientesAsync(
            creado.Id, new DateTime(2026, 11, 2, 12, 0, 0));

        var semanales = _contexto.PeriodosInteres
            .Where(x => x.PrestamoId == creado.Id && !x.EsCuotaFija)
            .ToList();

        Assert.NotEmpty(semanales);

        // 5% sobre el saldo (300 x 5% = 15.00).
        Assert.Contains(semanales, x => x.InteresGenerado == 15.00m);
    }
}
