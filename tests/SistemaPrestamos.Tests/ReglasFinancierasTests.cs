using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class ReglasFinancierasTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;
    private readonly ClienteService _clientes;
    private readonly PrestamoService _prestamos;
    private readonly PagoService _pagos;
    private readonly MorosidadService _morosidades;

    public ReglasFinancierasTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"test_{Guid.NewGuid()}")
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

        _clientes = new ClienteService(repoCliente, httpContext);

        _prestamos = new PrestamoService(
            repoPrestamo,
            repoCliente,
            repoPeriodo,
            repoPago);

        var periodos = new PeriodoInteresService(
            repoPrestamo,
            repoPeriodo);

        _pagos = new PagoService(
            repoPago,
            repoPrestamo,
            repoPeriodo,
            periodos);

        _morosidades = new MorosidadService(
            repoPrestamo,
            repoPeriodo,
            repoMorosidad,
            periodos);
    }

    public void Dispose()
    {
        _contexto.Dispose();
    }

    private async Task<Guid> CrearClienteActivoAsync(
        string documento = "70000001")
    {
        var dto = await _clientes.CrearAsync(new CrearClienteDto
        {
            TipoDocumento = "DNI",
            NumeroDocumento = documento,
            Nombres = "Test",
            Apellidos = "Uno",
            Telefono = "999111222",
            Direccion = "Av. Test 123"
        });

        return dto.Id;
    }

    private async Task<Guid> CrearPrestamoActivoAsync(
        Guid clienteId,
        decimal capital,
        DateTime? inicio = null)
    {
        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = clienteId,
            CapitalInicial = capital,
            FechaInicio = inicio ?? DateTime.UtcNow
        });

        Assert.Equal("Pendiente", creado.Estado);

        var aprobado = await _prestamos.AprobarAsync(creado.Id);

        Assert.True(aprobado);

        return creado.Id;
    }

    [Fact]
    public async Task Aprobar_GeneraPrimerPeriodo_ConInteres5Porciento()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 1000m);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);

        Assert.NotNull(prestamo);
        Assert.Equal("Activo", prestamo.Estado);
        Assert.Equal(50m, prestamo.CapitalInicial * prestamo.TasaInteresSemanal);
    }

    [Fact]
    public async Task Pago_SeAplicaPrimeroAInteres_RestoACapital()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 1000m);

        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 200m,
            FechaPago = DateTime.UtcNow
        });

        Assert.Equal(50m, pago.MontoInteres);
        Assert.Equal(150m, pago.MontoCapital);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);

        Assert.NotNull(prestamo);
        Assert.Equal(850m, prestamo.CapitalPendiente);
    }

    [Fact]
    public async Task PagoSoloInteres_NoReduceCapital()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 1000m);

        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 50m,
            FechaPago = DateTime.UtcNow
        });

        Assert.Equal(50m, pago.MontoInteres);
        Assert.Equal(0m, pago.MontoCapital);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);

        Assert.NotNull(prestamo);
        Assert.Equal(1000m, prestamo.CapitalPendiente);
    }

    [Fact]
    public async Task Prestamo_SeCancela_AlSaldarTodo()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 100m);

        // Interés 5 + capital 100.
        await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 105m,
            FechaPago = DateTime.UtcNow
        });

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);

        Assert.NotNull(prestamo);
        Assert.Equal("Cancelado", prestamo.Estado);
        Assert.Equal(0m, prestamo.CapitalPendiente);
    }

    [Fact]
    public async Task Prestamo_NoSeCancela_ConInteresPendiente()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 100m);

        // Solo capital: quedan 5 de interés.
        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 100m,
            FechaPago = DateTime.UtcNow
        });

        // 5 a interés + 95 a capital.
        Assert.Equal(5m, pago.MontoInteres);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);

        Assert.NotNull(prestamo);
        Assert.NotEqual("Cancelado", prestamo.Estado);
    }

    [Fact]
    public async Task PagoMayorADeuda_SeRechaza()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 100m);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _pagos.RegistrarAsync(new CrearPagoDto
            {
                PrestamoId = id,
                Monto = 99999m,
                FechaPago = DateTime.UtcNow
            }));
    }

    [Fact]
    public async Task Prestamo_ClienteNoActivo_SeRechaza()
    {
        var cliente = await CrearClienteActivoAsync();

        await _clientes.CambiarEstadoAsync(cliente, "Moroso");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _prestamos.CrearAsync(new CrearPrestamoDto
            {
                ClienteId = cliente,
                CapitalInicial = 500m,
                FechaInicio = DateTime.UtcNow
            }));
    }

    [Fact]
    public async Task Cliente_DocumentoDuplicado_SeRechaza()
    {
        await CrearClienteActivoAsync("70000099");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _clientes.CrearAsync(new CrearClienteDto
            {
                TipoDocumento = "DNI",
                NumeroDocumento = "70000099",
                Nombres = "Otro",
                Apellidos = "Dos",
                Telefono = "999333444"
            }));
    }

    [Fact]
    public async Task Mora_ConTresVencidos_SeActiva()
    {
        var cliente = await CrearClienteActivoAsync();
        var inicio = DateTime.UtcNow.AddDays(-28);

        var id = await CrearPrestamoActivoAsync(cliente, 500m, inicio);

        var mora = await _morosidades.EvaluarAsync(
            id,
            DateTime.UtcNow);

        Assert.NotNull(mora);
        Assert.True(mora.Activa);
        Assert.True(mora.PagosInteresVencidos >= 3);
    }

    [Fact]
    public async Task Mora_PrestamoAlDia_NoSeActiva()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 500m);

        var mora = await _morosidades.EvaluarAsync(
            id,
            DateTime.UtcNow);

        Assert.NotNull(mora);
        Assert.False(mora.Activa);
    }

    [Fact]
    public async Task Anular_Pendiente_QuedaAnulado()
    {
        var cliente = await CrearClienteActivoAsync();

        var creado = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 300m,
            FechaInicio = DateTime.UtcNow
        });

        var ok = await _prestamos.AnularAsync(
            creado.Id,
            "Cliente desistió del préstamo.");

        Assert.True(ok);

        var prestamo = await _prestamos.ObtenerPorIdAsync(creado.Id);

        Assert.NotNull(prestamo);
        Assert.Equal("Anulado", prestamo.Estado);
    }

    [Fact]
    public async Task Anular_ConPagos_SeRechaza()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 300m);

        await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 15m,
            FechaPago = DateTime.UtcNow
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _prestamos.AnularAsync(id, "Motivo válido de prueba."));
    }

    [Fact]
    public async Task Anular_SinMotivoValido_SeRechaza()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 300m);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _prestamos.AnularAsync(id, "corto"));
    }

    [Fact]
    public async Task Anular_Cancelado_SeRechaza()
    {
        var cliente = await CrearClienteActivoAsync();
        var id = await CrearPrestamoActivoAsync(cliente, 100m);

        await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 105m,
            FechaPago = DateTime.UtcNow
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _prestamos.AnularAsync(id, "Motivo válido de prueba."));
    }
}
