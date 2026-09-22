using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class DispositivosTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;
    private readonly DispositivoService _servicio;
    private readonly Guid _usuario = Guid.NewGuid();

    public DispositivosTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"test_{Guid.NewGuid()}")
            .Options;

        _contexto = new PrestamosDbContext(opciones);

        _servicio = new DispositivoService(
            new DispositivoRepository(_contexto));
    }

    public void Dispose()
    {
        _contexto.Dispose();
    }

    [Fact]
    public async Task Registrar_MasDeCinco_ConservaRecientes()
    {
        for (var i = 0; i < 7; i++)
        {
            await _servicio.RegistrarAsync(
                _usuario,
                $"token-{i}",
                "android");
        }

        var todos = await _servicio.ObtenerTodosAsync();

        Assert.Equal(5, todos.Count());
        Assert.DoesNotContain(todos, d => d.Token == "token-0");
        Assert.Contains(todos, d => d.Token == "token-6");
    }

    [Fact]
    public async Task EliminarPorToken_OtroUsuario_NoBorra()
    {
        await _servicio.RegistrarAsync(_usuario, "token-ajeno", "android");

        var ok = await _servicio.EliminarPorTokenAsync(
            Guid.NewGuid(),
            "token-ajeno");

        Assert.False(ok);
        Assert.Equal(1, (await _servicio.ObtenerTodosAsync()).Count());
    }

    [Fact]
    public async Task EliminarPorToken_Propio_Borra()
    {
        await _servicio.RegistrarAsync(_usuario, "token-propio", "android");

        var ok = await _servicio.EliminarPorTokenAsync(
            _usuario,
            "token-propio");

        Assert.True(ok);
        Assert.Empty(await _servicio.ObtenerTodosAsync());
    }
}
