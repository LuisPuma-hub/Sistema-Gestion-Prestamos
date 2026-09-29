using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class ImportacionTests : IDisposable
{
    private const string Cabecera =
        "idFila,tipoDoc,nroDoc,nombres,apellidos,telefono," +
        "direccion,capitalInicial,fechaInicio,saldoCapitalActual";

    private readonly PrestamosDbContext _contexto;
    private readonly ImportacionService _importacion;

    public ImportacionTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"import_{Guid.NewGuid()}")
            .Options;

        _contexto = new PrestamosDbContext(opciones);

        _importacion = new ImportacionService(
            new ClienteRepository(_contexto),
            new PrestamoRepository(_contexto),
            new PeriodoInteresRepository(_contexto),
            new PagoRepository(_contexto));
    }

    public void Dispose()
    {
        _contexto.Dispose();
    }

    private static string Csv(params string[] filas)
    {
        return Cabecera + "\n" + string.Join("\n", filas);
    }

    [Fact]
    public async Task Preview_TodoVerde_SinGuardar()
    {
        var csv = Csv(
            "1,DNI,70000001,Juan,Perez,987654321,Av. Lima 123," +
            "500.00,2026-08-01,400.00",
            "2,DNI,70000002,Maria,Lopez,912345678,Jr. Rosas 45," +
            "300.00,2026-09-01,");

        var corte = new DateTime(2026, 9, 27);

        var preview = await _importacion.PrevisualizarAsync(csv, corte);

        Assert.Equal(2, preview.TotalFilas);
        Assert.Equal(2, preview.Validas);
        Assert.Equal(0, preview.ConError);
        Assert.All(preview.Filas, x => Assert.True(x.Valida));

        // Sin saldo -> sin ajuste; con saldo menor -> con ajuste.
        Assert.Equal(0, preview.Filas[1].MontoAjuste);
        Assert.True(preview.Filas[0].MontoAjuste > 0);
        Assert.True(preview.Filas[0].PeriodosGenerados > 0);

        // El preview no guarda nada.
        Assert.Empty(_contexto.Clientes);
        Assert.Empty(_contexto.Prestamos);
    }

    [Fact]
    public async Task Preview_ReportaErrores_PorLinea()
    {
        var csv = Csv(
            "1,DNI,70000001,Juan,Perez,123,Av. Lima,500,2026-08-01,",
            "2,DNI,70000001,Ana,Diaz,987654321,,200,2026-08-01,",
            "3,DNI,ABC,Pepe,Lopez,987654321,,100,2030-01-01,");

        var preview = await _importacion.PrevisualizarAsync(
            csv, new DateTime(2026, 9, 27));

        Assert.Equal(2, preview.ConError);
        Assert.Contains("Teléfono", preview.Filas[0].Error);
        Assert.True(preview.Filas[1].Valida);
        Assert.Contains("DNI", preview.Filas[2].Error);
    }

    [Fact]
    public async Task Confirm_GuardaTodo_ConAjuste()
    {
        var csv = Csv(
            "1,DNI,70000001,Juan,Perez,987654321,Av. Lima 123," +
            "500.00,2026-08-01,400.00");

        var corte = new DateTime(2026, 9, 27);

        var resultado = await _importacion.ConfirmarAsync(csv, corte);

        Assert.Equal(1, resultado.PrestamosCreados);
        Assert.Equal(1, resultado.ClientesCreados);
        Assert.Equal(1, resultado.PagosAjuste);
        Assert.True(resultado.MontoAjusteTotal > 0);

        var prestamo = _contexto.Prestamos.Single();
        Assert.Equal("Activo", prestamo.Estado);
        Assert.Equal(400.00m, prestamo.CapitalPendiente);

        var periodos = _contexto.PeriodosInteres
            .Where(x => x.PrestamoId == prestamo.Id)
            .ToList();
        Assert.Equal(resultado.PeriodosGenerados, periodos.Count);
        Assert.All(periodos, x => Assert.Equal("Pagado", x.Estado));

        var pago = _contexto.Pagos.Single();
        Assert.Equal(resultado.MontoAjusteTotal, pago.Monto);
        Assert.Equal(
            "CARGA INICIAL - importación masiva sin historial",
            pago.Observaciones);
    }

    [Fact]
    public async Task Confirm_SinSaldo_QuedaTodoPendiente()
    {
        var csv = Csv(
            "1,DNI,70000001,Juan,Perez,987654321,,500.00,2026-09-20,");

        var resultado = await _importacion.ConfirmarAsync(
            csv, new DateTime(2026, 9, 27));

        Assert.Equal(0, resultado.PagosAjuste);

        var prestamo = _contexto.Prestamos.Single();
        Assert.Equal(500.00m, prestamo.CapitalPendiente);
        Assert.All(
            _contexto.PeriodosInteres,
            x => Assert.Equal("Pendiente", x.Estado));
    }

    [Fact]
    public async Task Confirm_ConError_NoGuardaNada()
    {
        var csv = Csv(
            "1,DNI,70000001,Juan,Perez,987654321,,500.00,2026-08-01,",
            "2,DNI,12,Mal,Dato,000,,0,futura,");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _importacion.ConfirmarAsync(
                csv, new DateTime(2026, 9, 27)));

        Assert.Empty(_contexto.Clientes);
        Assert.Empty(_contexto.Prestamos);
        Assert.Empty(_contexto.PeriodosInteres);
        Assert.Empty(_contexto.Pagos);
    }

    [Fact]
    public async Task Confirm_ReutilizaClienteExistente()
    {
        _contexto.Clientes.Add(new Domain.Entities.Cliente
        {
            Id = Guid.NewGuid(),
            TipoDocumento = "DNI",
            NumeroDocumento = "70000001",
            Nombres = "Juan",
            Apellidos = "Perez",
            Telefono = "987654321",
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        });
        await _contexto.SaveChangesAsync();
        _contexto.ChangeTracker.Clear();

        var csv = Csv(
            "1,DNI,70000001,Juan,Perez,987654321,,500.00,2026-08-01,");

        var resultado = await _importacion.ConfirmarAsync(
            csv, new DateTime(2026, 9, 27));

        Assert.Equal(0, resultado.ClientesCreados);
        Assert.Equal(1, resultado.ClientesReutilizados);
        Assert.Single(_contexto.Clientes);
    }

    [Fact]
    public async Task Preview_Rechaza_NombreDistintoParaMismoDni()
    {
        _contexto.Clientes.Add(new Domain.Entities.Cliente
        {
            Id = Guid.NewGuid(),
            TipoDocumento = "DNI",
            NumeroDocumento = "70000001",
            Nombres = "Otro",
            Apellidos = "Nombre",
            Telefono = "987654321",
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        });
        await _contexto.SaveChangesAsync();
        _contexto.ChangeTracker.Clear();

        var csv = Csv(
            "1,DNI,70000001,Juan,Perez,987654321,,500.00,2026-08-01,");

        var preview = await _importacion.PrevisualizarAsync(
            csv, new DateTime(2026, 9, 27));

        Assert.Equal(1, preview.ConError);
        Assert.Contains("otro nombre", preview.Filas[0].Error);
    }
}
