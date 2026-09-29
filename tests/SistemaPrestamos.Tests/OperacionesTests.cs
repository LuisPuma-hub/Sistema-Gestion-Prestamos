using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class OperacionesTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;
    private readonly ClienteService _clientes;
    private readonly PrestamoService _prestamos;
    private readonly PagoService _pagos;
    private readonly MorosidadService _morosidades;
    private readonly UsuarioService _usuarios;
    private readonly AuthService _auth;
    private readonly ReporteService _reportes;

    public OperacionesTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"op_{Guid.NewGuid()}")
            .Options;

        _contexto = new PrestamosDbContext(opciones);

        var repoCliente = new ClienteRepository(_contexto);
        var repoPrestamo = new PrestamoRepository(_contexto);
        var repoPago = new PagoRepository(_contexto);
        var repoPeriodo = new PeriodoInteresRepository(_contexto);
        var repoMorosidad = new MorosidadRepository(_contexto);
        var repoUsuario = new UsuarioRepository(_contexto);
        var repoRefresh = new RefreshTokenRepository(_contexto);
        var repoReset = new PasswordResetRepository(_contexto);

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

        var periodos = new PeriodoInteresService(
            repoPrestamo,
            repoPeriodo);

        _morosidades = new MorosidadService(
            repoPrestamo,
            repoPeriodo,
            repoMorosidad,
            periodos);

        _pagos = new PagoService(
            repoPago,
            repoPrestamo,
            repoPeriodo,
            periodos,
            null,
            repoMorosidad,
            _morosidades);

        _usuarios = new UsuarioService(repoUsuario, repoReset);
        _auth = new AuthService(repoUsuario, repoRefresh, null!, repoReset);

        _reportes = new ReporteService(
            repoPago, repoPrestamo, repoPeriodo);
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
            Apellidos = "Op",
            Telefono = "987654321"
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

        await _prestamos.AprobarAsync(creado.Id);

        return creado.Id;
    }

    [Fact]
    public async Task Anular_RevierteSaldos()
    {
        var cliente = await CrearClienteAsync("90000001");
        var id = await CrearPrestamoActivoAsync(cliente, 1000m);

        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 200m,
            FechaPago = DateTime.UtcNow
        });

        Assert.Contains("P1", pago.Detalle);
        Assert.Contains("Capital", pago.Detalle);

        var ok = await _pagos.AnularAsync(
            pago.Id, "Monto digitado incorrectamente.", null);

        Assert.True(ok);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(prestamo);
        Assert.Equal(1000m, prestamo.CapitalPendiente);

        var guardado = await _pagos.ObtenerPorIdAsync(pago.Id);
        Assert.NotNull(guardado);
        Assert.Equal("Anulado", guardado.Estado);
        Assert.Equal(
            "Monto digitado incorrectamente.",
            guardado.MotivoAnulacion);
    }

    [Fact]
    public async Task Anular_Doble_SeRechaza()
    {
        var cliente = await CrearClienteAsync("90000002");
        var id = await CrearPrestamoActivoAsync(cliente, 500m);

        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 50m,
            FechaPago = DateTime.UtcNow
        });

        await _pagos.AnularAsync(pago.Id, "Motivo válido uno.", null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _pagos.AnularAsync(
                pago.Id, "Motivo válido dos.", null));
    }

    [Fact]
    public async Task Anular_MotivoCorto_SeRechaza()
    {
        var cliente = await CrearClienteAsync("90000003");
        var id = await CrearPrestamoActivoAsync(cliente, 500m);

        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 50m,
            FechaPago = DateTime.UtcNow
        });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _pagos.AnularAsync(pago.Id, "corto", null));
    }

    [Fact]
    public async Task Anular_Cancelado_Reabre()
    {
        var cliente = await CrearClienteAsync("90000004");
        var id = await CrearPrestamoActivoAsync(cliente, 100m);

        var periodos = _contexto.PeriodosInteres
            .Where(x => x.PrestamoId == id)
            .ToList();
        var deuda = 100m + periodos.Sum(x => x.InteresPendiente);

        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = deuda,
            FechaPago = DateTime.UtcNow
        });

        var cancelado = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(cancelado);
        Assert.Equal("Cancelado", cancelado.Estado);

        await _pagos.AnularAsync(pago.Id, "Pago duplicado del día.", null);

        var reabierto = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(reabierto);
        Assert.Equal("Activo", reabierto.Estado);
        Assert.Equal(100m, reabierto.CapitalPendiente);
    }

    [Fact]
    public async Task Reset_GenerarYCanjear_Ok()
    {
        var usuario = await _usuarios.CrearAsync(new CrearUsuarioDto
        {
            Nombres = "Ana",
            Apellidos = "Reset",
            Email = "ana@reset.com",
            Password = "12345678",
            Rol = "Cobrador"
        });

        var reset = await _usuarios.GenerarTokenReseteoAsync(
            usuario.Id);

        Assert.False(string.IsNullOrWhiteSpace(reset.Token));

        await _auth.CanjearResetAsync(reset.Token, "nueva1234");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _auth.CanjearResetAsync(reset.Token, "otra1234"));
    }

    [Fact]
    public async Task Reset_TokenInvalido_SeRechaza()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _auth.CanjearResetAsync("no-existe", "nueva1234"));
    }

    [Fact]
    public async Task Job_EvaluarVigentes_MarcaMora()
    {
        var cliente = await CrearClienteAsync("90000005");
        await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        var total = await _morosidades.EvaluarVigentesAsync(
            DateTime.UtcNow);

        Assert.True(total >= 1);

        var cli = await _clientes.ObtenerPorIdAsync(cliente);
        Assert.NotNull(cli);
        Assert.Equal("Moroso", cli.Estado);
    }

    [Fact]
    public async Task Reporte_Cobranza_ExcluyeAnulados()
    {
        var cliente = await CrearClienteAsync("90000006");
        var id = await CrearPrestamoActivoAsync(cliente, 500m);

        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 50m,
            FechaPago = DateTime.UtcNow
        });

        var antes = await _reportes.CobranzaDelDiaAsync(
            DateTime.UtcNow);

        Assert.Equal(50m, antes.Total);
        Assert.Equal(1, antes.NumPagos);

        await _pagos.AnularAsync(pago.Id, "Anulación de prueba.", null);

        var despues = await _reportes.CobranzaDelDiaAsync(
            DateTime.UtcNow);

        Assert.Equal(0m, despues.Total);
        Assert.Equal(0, despues.NumPagos);
    }

    [Fact]
    public async Task Reporte_Cartera_Totales()
    {
        var cliente = await CrearClienteAsync("90000007");
        await CrearPrestamoActivoAsync(cliente, 200m);

        var cartera = await _reportes.CarteraAsync();

        Assert.Equal(1, cartera.NumPrestamos);
        Assert.Equal(200m, cartera.CapitalTotal);
        Assert.True(cartera.InteresesTotal > 0);
    }

    [Fact]
    public async Task Reporte_Ingresos_RangoInvalido_SeRechaza()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _reportes.IngresosAsync(
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(-1)));
    }
}
