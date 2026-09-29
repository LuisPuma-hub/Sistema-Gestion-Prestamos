using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class MorosidadEstadosTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;
    private readonly ClienteService _clientes;
    private readonly PrestamoService _prestamos;
    private readonly PagoService _pagos;
    private readonly MorosidadService _morosidades;

    public MorosidadEstadosTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"mora_{Guid.NewGuid()}")
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

        var periodos = new PeriodoInteresService(
            repoPrestamo,
            repoPeriodo);

        _pagos = new PagoService(
            repoPago,
            repoPrestamo,
            repoPeriodo,
            periodos,
            null,
            repoMorosidad);

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

    private async Task<Guid> CrearClienteAsync(string documento)
    {
        var dto = await _clientes.CrearAsync(new CrearClienteDto
        {
            TipoDocumento = "DNI",
            NumeroDocumento = documento,
            Nombres = "Test",
            Apellidos = "Mora",
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
    public async Task Crear_SegundoPrestamoActivo_Permitido()
    {
        var cliente = await CrearClienteAsync("80000001");

        await CrearPrestamoActivoAsync(cliente, 500m);

        // RN-CLI-008: con cliente Activo se permiten varios
        // vigentes; el freno es la mora, no la cantidad.
        var segundo = await _prestamos.CrearAsync(new CrearPrestamoDto
        {
            ClienteId = cliente,
            CapitalInicial = 300m,
            FechaInicio = DateTime.UtcNow
        });

        Assert.Equal("Pendiente", segundo.Estado);
    }

    [Fact]
    public async Task Crear_ClienteMoroso_SeRechaza()
    {
        var cliente = await CrearClienteAsync("80000002");

        await _clientes.CambiarEstadoAsync(cliente, "Moroso");

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _prestamos.CrearAsync(new CrearPrestamoDto
            {
                ClienteId = cliente,
                CapitalInicial = 300m,
                FechaInicio = DateTime.UtcNow
            }));

        Assert.Contains("mora", ex.Message);
    }

    [Fact]
    public async Task Evaluar_TresVencidos_PoneMoroso()
    {
        var cliente = await CrearClienteAsync("80000003");
        var id = await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        var mora = await _morosidades.EvaluarAsync(id, DateTime.UtcNow);

        Assert.True(mora.Activa);
        Assert.True(mora.PagosInteresVencidos >= 3);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(prestamo);
        Assert.Equal("Moroso", prestamo.Estado);

        var cli = await _clientes.ObtenerPorIdAsync(cliente);
        Assert.NotNull(cli);
        Assert.Equal("Moroso", cli.Estado);
    }

    [Fact]
    public async Task Reactivar_ClienteQuedaEnObservacion()
    {
        var cliente = await CrearClienteAsync("80000004");
        var id = await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        await _morosidades.EvaluarAsync(id, DateTime.UtcNow);

        var mora = await _morosidades.ReactivarAsync(
            id, "Cliente regularizó su situación.");

        Assert.False(mora.Activa);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(prestamo);
        Assert.Equal("Activo", prestamo.Estado);

        var cli = await _clientes.ObtenerPorIdAsync(cliente);
        Assert.NotNull(cli);
        Assert.Equal("En observación", cli.Estado);
    }

    [Fact]
    public async Task Pago_EnPrestamoMoroso_ExigeMinimo()
    {
        var cliente = await CrearClienteAsync("80000005");
        var id = await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        await _morosidades.EvaluarAsync(id, DateTime.UtcNow);

        // 4 semanas vencidas x S/ 5 = mínimo S/ 20.
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _pagos.RegistrarAsync(new CrearPagoDto
            {
                PrestamoId = id,
                Monto = 5m,
                FechaPago = DateTime.UtcNow
            }));

        Assert.Contains("mínimo", ex.Message);
    }

    [Fact]
    public async Task Pago_LimpiaMora_ReactivaAutomatico()
    {
        var cliente = await CrearClienteAsync("80000055");
        var id = await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        await _morosidades.EvaluarAsync(id, DateTime.UtcNow);

        var pago = await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = 20m,
            FechaPago = DateTime.UtcNow
        });

        Assert.Equal(20m, pago.Monto);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(prestamo);
        Assert.Equal("Activo", prestamo.Estado);

        var cli = await _clientes.ObtenerPorIdAsync(cliente);
        Assert.NotNull(cli);
        Assert.Equal("En observación", cli.Estado);

        var mora = await _morosidades.ObtenerPorPrestamoAsync(id);
        Assert.NotNull(mora);
        Assert.False(mora.Activa);
    }

    [Fact]
    public async Task PagoTotal_ClienteVuelveAActivo()
    {
        var cliente = await CrearClienteAsync("80000006");
        var id = await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        await _morosidades.EvaluarAsync(id, DateTime.UtcNow);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(prestamo);
        Assert.Equal("Moroso", prestamo.Estado);

        var periodos = _contexto.PeriodosInteres
            .Where(x => x.PrestamoId == id)
            .ToList();
        var deuda = prestamo.CapitalPendiente +
                    periodos.Sum(x => x.InteresPendiente);

        await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = id,
            Monto = deuda,
            FechaPago = DateTime.UtcNow
        });

        prestamo = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(prestamo);
        Assert.Equal("Cancelado", prestamo.Estado);

        var cli = await _clientes.ObtenerPorIdAsync(cliente);
        Assert.NotNull(cli);
        Assert.Equal("En observación", cli.Estado);

        var mora = await _morosidades.ObtenerPorPrestamoAsync(id);
        Assert.NotNull(mora);
        Assert.False(mora.Activa);
    }

    private async Task<Guid> CrearPendienteDirectoAsync(
        Guid clienteId,
        decimal capital)
    {
        var pendiente = new Prestamo
        {
            Id = Guid.NewGuid(),
            ClienteId = clienteId,
            CapitalInicial = capital,
            TasaInteresSemanal = 0.05m,
            CapitalPendiente = capital,
            FechaInicio = DateTime.UtcNow,
            FechaAprobacion = null,
            Estado = "Pendiente"
        };

        _contexto.Prestamos.Add(pendiente);
        await _contexto.SaveChangesAsync();
        _contexto.ChangeTracker.Clear();

        return pendiente.Id;
    }

    [Fact]
    public async Task Aprobar_ClienteMoroso_SeRechaza()
    {
        var cliente = await CrearClienteAsync("80000007");
        await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        var moroso = _contexto.Prestamos
            .Single(x => x.ClienteId == cliente);
        await _morosidades.EvaluarAsync(moroso.Id, DateTime.UtcNow);

        var pendiente = await CrearPendienteDirectoAsync(cliente, 200m);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _prestamos.AprobarAsync(pendiente));

        Assert.Contains("no está activo", ex.Message);
    }

    [Fact]
    public async Task Aprobar_ConOtroVigente_Permitido()
    {
        var cliente = await CrearClienteAsync("80000008");
        await CrearPrestamoActivoAsync(cliente, 100m);

        var pendiente = await CrearPendienteDirectoAsync(cliente, 200m);

        var ok = await _prestamos.AprobarAsync(pendiente);

        Assert.True(ok);
    }

    [Fact]
    public async Task CambiarEstado_ConMora_SeRechaza()
    {
        var cliente = await CrearClienteAsync("80000009");
        var id = await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        await _morosidades.EvaluarAsync(id, DateTime.UtcNow);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _clientes.CambiarEstadoAsync(cliente, "Activo"));

        Assert.Contains("mora", ex.Message);

        var cli = await _clientes.ObtenerPorIdAsync(cliente);
        Assert.NotNull(cli);
        Assert.Equal("Moroso", cli.Estado);
    }

    [Fact]
    public async Task CambiarEstado_SinMora_Permitido()
    {
        var cliente = await CrearClienteAsync("80000010");

        var ok = await _clientes.CambiarEstadoAsync(
            cliente, "En observación");

        Assert.True(ok);

        ok = await _clientes.CambiarEstadoAsync(cliente, "Activo");

        Assert.True(ok);
    }
}
