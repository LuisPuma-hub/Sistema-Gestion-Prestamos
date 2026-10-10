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
    public async Task Crear_DefectoEsFijo()
    {
        var cliente = await CrearClienteAsync("81000001");

        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 500m,
            FechaInicio = DateTime.UtcNow
        });

        Assert.Equal("Fijo", creado.EsquemaInteres);
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
}
