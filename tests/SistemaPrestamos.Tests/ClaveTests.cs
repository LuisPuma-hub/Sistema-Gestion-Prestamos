using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class ClaveTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;
    private readonly UsuarioService _usuarios;

    public ClaveTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"test_{Guid.NewGuid()}")
            .Options;

        _contexto = new PrestamosDbContext(opciones);

        _usuarios = new UsuarioService(
            new UsuarioRepository(_contexto));
    }

    public void Dispose()
    {
        _contexto.Dispose();
    }

    private async Task<Guid> CrearCobradorAsync(
        string email = "clave@test.local")
    {
        var dto = await _usuarios.CrearAsync(new CrearUsuarioDto
        {
            Nombres = "Clave",
            Apellidos = "Test",
            Email = email,
            Password = "Inicial123!",
            Rol = "Cobrador"
        });

        return dto.Id;
    }

    [Fact]
    public async Task CambiarClave_ConActualCorrecta_Funciona()
    {
        var id = await CrearCobradorAsync();

        await _usuarios.CambiarClaveAsync(id, "Inicial123!", "NuevaClave123!");

        // La nueva pasa a ser la actual.
        await _usuarios.CambiarClaveAsync(id, "NuevaClave123!", "OtraClave123!");
    }

    [Fact]
    public async Task CambiarClave_ConActualIncorrecta_SeRechaza()
    {
        var id = await CrearCobradorAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _usuarios.CambiarClaveAsync(id, "Equivocada123!", "NuevaClave123!"));
    }

    [Fact]
    public async Task CambiarClave_Corta_SeRechaza()
    {
        var id = await CrearCobradorAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _usuarios.CambiarClaveAsync(id, "Inicial123!", "corta"));
    }

    [Fact]
    public async Task ResetearClave_NoExigeActual()
    {
        var id = await CrearCobradorAsync("reset@test.local");

        await _usuarios.ResetearClaveAsync(id, "Reseteada123!");

        await _usuarios.CambiarClaveAsync(id, "Reseteada123!", "Final12345!");
    }
}
