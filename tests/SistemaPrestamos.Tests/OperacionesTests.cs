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

    [Fact]
    public async Task Ajustar_Capital_ReduceYCreaRegistro()
    {
        var cliente = await CrearClienteAsync("90000009");
        var id = await CrearPrestamoActivoAsync(cliente, 1000m);

        var ajuste = await _pagos.AjustarAsync(
            id,
            new AjustarPrestamoDto
            {
                NuevoCapital = 800m,
                PerdonarIntereses = false,
                Motivo = "Descuento pactado por pronto pago."
            },
            null);

        Assert.Equal("Ajuste", ajuste.Estado);
        Assert.Equal(200m, ajuste.Monto);
        Assert.Equal(200m, ajuste.MontoCapital);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(prestamo);
        Assert.Equal(800m, prestamo.CapitalPendiente);

        var cobranza = await _reportes.CobranzaDelDiaAsync(
            DateTime.UtcNow);
        Assert.Equal(0m, cobranza.Total);
    }

    [Fact]
    public async Task Ajustar_Parcial_PerdonaHastaMonto()
    {
        var cliente = await CrearClienteAsync("90000021");
        var id = await CrearPrestamoActivoAsync(
            cliente, 1000m, DateTime.UtcNow.AddDays(-21));

        // 3 semanas vencidas x S/ 50 = S/ 150.
        var ajuste = await _pagos.AjustarAsync(
            id,
            new AjustarPrestamoDto
            {
                PerdonarIntereses = true,
                MontoPerdonar = 60m,
                Motivo = "Descuento por mora prolongada."
            },
            null);

        Assert.Equal("Ajuste", ajuste.Estado);
        Assert.Equal(60m, ajuste.Monto);
        Assert.Equal(60m, ajuste.MontoInteres);
        Assert.Equal(0m, ajuste.MontoCapital);
    }

    [Fact]
    public async Task Ajustar_Parcial_SuperaVencido_SeRechaza()
    {
        var cliente = await CrearClienteAsync("90000022");
        var id = await CrearPrestamoActivoAsync(
            cliente, 1000m, DateTime.UtcNow.AddDays(-14));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _pagos.AjustarAsync(
                id,
                new AjustarPrestamoDto
                {
                    PerdonarIntereses = true,
                    MontoPerdonar = 99999m,
                    Motivo = "Descuento por mora prolongada."
                },
                null));
    }

    [Fact]
    public async Task Ajustar_SubirCapital_SeRechaza()
    {
        var cliente = await CrearClienteAsync("90000010");
        var id = await CrearPrestamoActivoAsync(cliente, 1000m);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _pagos.AjustarAsync(
                id,
                new AjustarPrestamoDto
                {
                    NuevoCapital = 1200m,
                    PerdonarIntereses = false,
                    Motivo = "Motivo válido de prueba."
                },
                null));
    }

    [Fact]
    public async Task Ajustar_MotivoCorto_SeRechaza()
    {
        var cliente = await CrearClienteAsync("90000011");
        var id = await CrearPrestamoActivoAsync(cliente, 1000m);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _pagos.AjustarAsync(
                id,
                new AjustarPrestamoDto
                {
                    NuevoCapital = 900m,
                    PerdonarIntereses = false,
                    Motivo = "corto"
                },
                null));
    }

    [Fact]
    public async Task Ajustar_Condona_SaleDeMora()
    {
        var cliente = await CrearClienteAsync("90000012");
        var id = await CrearPrestamoActivoAsync(
            cliente, 100m, DateTime.UtcNow.AddDays(-28));

        await _morosidades.EvaluarAsync(id, DateTime.UtcNow);

        var mora = await _pagos.AjustarAsync(
            id,
            new AjustarPrestamoDto
            {
                NuevoCapital = null,
                PerdonarIntereses = true,
                Motivo = "Condonación autorizada por gerencia."
            },
            null);

        Assert.Equal("Ajuste", mora.Estado);
        Assert.True(mora.Monto > 0);

        var prestamo = await _prestamos.ObtenerPorIdAsync(id);
        Assert.NotNull(prestamo);
        Assert.Equal("Activo", prestamo.Estado);

        var cli = await _clientes.ObtenerPorIdAsync(cliente);
        Assert.NotNull(cli);
        Assert.Equal("En observación", cli.Estado);
    }

    [Fact]
    public async Task Ajustar_SinCambios_SeRechaza()
    {
        var cliente = await CrearClienteAsync("90000013");
        var id = await CrearPrestamoActivoAsync(cliente, 1000m);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _pagos.AjustarAsync(
                id,
                new AjustarPrestamoDto
                {
                    NuevoCapital = 1000m,
                    PerdonarIntereses = false,
                    Motivo = "Motivo válido de prueba."
                },
                null));
    }

    private ReporteService CrearReportesConFondo()
    {
        return new ReporteService(
            new PagoRepository(_contexto),
            new PrestamoRepository(_contexto),
            new PeriodoInteresRepository(_contexto),
            new FondoRepository(_contexto));
    }

    [Fact]
    public async Task Capital_Resumen_Calcula()
    {
        var reportes = CrearReportesConFondo();
        var cliente = await CrearClienteAsync("90000008");

        await reportes.RegistrarMovimientoAsync(new CrearFondoMovimientoDto
        {
            Tipo = "Aporte",
            Monto = 10000m,
            Fecha = DateTime.UtcNow,
            Motivo = "Fondo inicial"
        });

        var prestamo = await CrearPrestamoActivoAsync(cliente, 1000m);

        await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = prestamo,
            Monto = 200m,
            FechaPago = DateTime.UtcNow
        });

        var capital = await reportes.CapitalAsync();

        Assert.Equal(10000m, capital.Aportes);
        Assert.Equal(0m, capital.Retiros);
        Assert.Equal(10000m, capital.BaseEfectiva);
        Assert.Equal(850m, capital.Colocado);
        Assert.Equal(50m, capital.GanadoIntereses);
        Assert.Equal(150m, capital.CapitalRecuperado);
        Assert.Equal(9150m, capital.Disponible);
        Assert.Equal(0.5m, capital.Roi);
    }

    [Fact]
    public async Task Movimiento_TipoInvalido_SeRechaza()
    {
        var reportes = CrearReportesConFondo();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => reportes.RegistrarMovimientoAsync(
                new CrearFondoMovimientoDto
                {
                    Tipo = "Prestamo",
                    Monto = 100m,
                    Fecha = DateTime.UtcNow
                }));
    }

    [Fact]
    public async Task Movimiento_Eliminar_Ok()
    {
        var reportes = CrearReportesConFondo();

        var creado = await reportes.RegistrarMovimientoAsync(
            new CrearFondoMovimientoDto
            {
                Tipo = "Retiro",
                Monto = 500m,
                Fecha = DateTime.UtcNow
            });

        Assert.True(await reportes.EliminarMovimientoAsync(creado.Id));
        Assert.False(await reportes.EliminarMovimientoAsync(Guid.NewGuid()));

        var capital = await reportes.CapitalAsync();
        Assert.Equal(0m, capital.Retiros);
    }

    [Fact]
    public async Task Compromiso_Crear_Y_Anular_Ok()
    {
        var cliente = await CrearClienteAsync("93000001");
        var repo = new NotaCompromisoRepository(_contexto);
        var servicio = new CompromisoService(
            repo, new ClienteRepository(_contexto));

        var creado = await servicio.CrearAsync(
            cliente,
            new CrearCompromisoDto
            {
                Texto = "Paga el lunes",
                FechaCompromiso = DateTime.UtcNow.Date
            },
            null);

        Assert.Equal("Pendiente", creado.Estado);

        var lista = await servicio.ListarPorClienteAsync(cliente);
        Assert.Single(lista);

        Assert.True(await servicio.AnularAsync(creado.Id));
    }

    [Fact]
    public async Task Compromiso_TextoVacio_SeRechaza()
    {
        var cliente = await CrearClienteAsync("93000002");
        var servicio = new CompromisoService(
            new NotaCompromisoRepository(_contexto),
            new ClienteRepository(_contexto));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => servicio.CrearAsync(
                cliente,
                new CrearCompromisoDto
                {
                    Texto = " ",
                    FechaCompromiso = DateTime.UtcNow.Date
                },
                null));
    }

    [Fact]
    public async Task Compromiso_SeCumple_ConPago()
    {
        var cliente = await CrearClienteAsync("93000003");
        var repo = new NotaCompromisoRepository(_contexto);
        var compromisos = new CompromisoService(
            repo, new ClienteRepository(_contexto));

        await compromisos.CrearAsync(
            cliente,
            new CrearCompromisoDto
            {
                Texto = "Paga hoy",
                FechaCompromiso = DateTime.UtcNow.Date
            },
            null);

        var prestamo = await CrearPrestamoActivoAsync(cliente, 500m);

        var pagos = new PagoService(
            new PagoRepository(_contexto),
            new PrestamoRepository(_contexto),
            new PeriodoInteresRepository(_contexto),
            new PeriodoInteresService(
                new PrestamoRepository(_contexto),
                new PeriodoInteresRepository(_contexto)),
            null, null, null,
            compromisos);

        await pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = prestamo,
            Monto = 25m,
            FechaPago = DateTime.UtcNow
        });

        var lista = await compromisos.ListarPorClienteAsync(cliente);

        Assert.Equal("Cumplida", lista.Single().Estado);
    }

    [Fact]
    public async Task Compromiso_PorPrestamo_SoloCierraEse()
    {
        var cliente = await CrearClienteAsync("93000005");
        var repo = new NotaCompromisoRepository(_contexto);
        var prestamoRepo = new PrestamoRepository(_contexto);
        var servicio = new CompromisoService(
            repo, new ClienteRepository(_contexto), prestamoRepo);

        var prestamoA = await CrearPrestamoActivoAsync(cliente, 500m);
        var prestamoB = await CrearPrestamoActivoAsync(cliente, 400m);

        await servicio.CrearAsync(
            cliente,
            new CrearCompromisoDto
            {
                Texto = "Paga el A",
                FechaCompromiso = DateTime.UtcNow.Date,
                PrestamoId = prestamoA
            },
            null);

        var pagos = new PagoService(
            new PagoRepository(_contexto),
            prestamoRepo,
            new PeriodoInteresRepository(_contexto),
            new PeriodoInteresService(
                prestamoRepo,
                new PeriodoInteresRepository(_contexto)),
            null, null, null,
            servicio);

        // Paga el B: el compromiso del A sigue pendiente.
        await pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = prestamoB,
            Monto = 20m,
            FechaPago = DateTime.UtcNow
        });

        Assert.Equal(
            "Pendiente",
            (await servicio.ListarPorClienteAsync(cliente))
                .Single().Estado);

        // Paga el A: se cumple.
        await pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = prestamoA,
            Monto = 25m,
            FechaPago = DateTime.UtcNow
        });

        Assert.Equal(
            "Cumplida",
            (await servicio.ListarPorClienteAsync(cliente))
                .Single().Estado);
    }

    [Fact]
    public async Task Compromiso_Pasado_SeMarcaVencido()
    {
        var cliente = await CrearClienteAsync("93000004");
        var servicio = new CompromisoService(
            new NotaCompromisoRepository(_contexto),
            new ClienteRepository(_contexto));

        await servicio.CrearAsync(
            cliente,
            new CrearCompromisoDto
            {
                Texto = "Pagaba ayer",
                FechaCompromiso = DateTime.UtcNow.Date.AddDays(-1)
            },
            null);

        var hoy = ProgramadorService.AhoraLima(DateTime.UtcNow).Date;

        await servicio.MarcarVencidosAsync(hoy);

        var lista = await servicio.ListarPorClienteAsync(cliente);

        Assert.Equal("Vencida", lista.Single().Estado);
    }

    [Fact]
    public async Task Resumen_General_Calcula()
    {
        var reportes = CrearReportesConFondo();
        var cliente = await CrearClienteAsync("90000009");

        await reportes.RegistrarMovimientoAsync(new CrearFondoMovimientoDto
        {
            Tipo = "Aporte",
            Monto = 10000m,
            Fecha = DateTime.UtcNow,
            Motivo = "Fondo inicial"
        });

        var prestamo = await CrearPrestamoActivoAsync(cliente, 1000m);

        await _pagos.RegistrarAsync(new CrearPagoDto
        {
            PrestamoId = prestamo,
            Monto = 200m,
            FechaPago = DateTime.UtcNow
        });

        var resumen = await reportes.ResumenGeneralAsync();

        Assert.Equal(850m, resumen.CapitalTotal);
        Assert.Equal(0m, resumen.InteresesPendientes);
        Assert.Equal(50m, resumen.InteresesGanados);
        Assert.Equal(150m, resumen.CapitalRecuperado);
        Assert.Equal(0m, resumen.Reinvertido);
        Assert.Equal(1, resumen.NumPrestamos);
    }

    [Fact]
    public async Task Resumen_Reinvertido_CuandoColocadoSuperaBase()
    {
        var reportes = CrearReportesConFondo();
        var cliente = await CrearClienteAsync("90000010");

        await reportes.RegistrarMovimientoAsync(new CrearFondoMovimientoDto
        {
            Tipo = "Aporte",
            Monto = 500m,
            Fecha = DateTime.UtcNow,
            Motivo = "Fondo inicial"
        });

        await CrearPrestamoActivoAsync(cliente, 1000m);

        var resumen = await reportes.ResumenGeneralAsync();

        Assert.Equal(1000m, resumen.CapitalTotal);
        Assert.Equal(500m, resumen.BaseEfectiva);
        Assert.Equal(500m, resumen.Reinvertido);
    }
}
