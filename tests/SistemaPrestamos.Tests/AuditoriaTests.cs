using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class AuditoriaTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;

    public AuditoriaTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"test_{Guid.NewGuid()}")
            .Options;

        _contexto = new PrestamosDbContext(opciones);
    }

    public void Dispose()
    {
        _contexto.Dispose();
    }

    private List<Auditoria> Auditorias() =>
        _contexto.Auditorias.AsNoTracking().ToList();

    [Fact]
    public async Task Crear_GeneraFilaInsert_ConDespues()
    {
        _contexto.Clientes.Add(new Cliente
        {
            Id = Guid.NewGuid(),
            TipoDocumento = "DNI",
            NumeroDocumento = "80000001",
            Nombres = "Audit",
            Apellidos = "Uno",
            Telefono = "999000111",
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        });

        await _contexto.SaveChangesAsync();

        var filas = Auditorias();

        var fila = Assert.Single(filas, f => f.Operacion == "ADDED");
        Assert.Equal("clientes", fila.Tabla);

        using var doc = JsonDocument.Parse(fila.Diff!);

        Assert.True(doc.RootElement
            .GetProperty("despues")
            .TryGetProperty("Nombres", out _));
    }

    [Fact]
    public async Task Modificar_GeneraFilaUpdate_SoloCamposCambiados()
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            TipoDocumento = "DNI",
            NumeroDocumento = "80000002",
            Nombres = "Audit",
            Apellidos = "Dos",
            Telefono = "999000222",
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        };

        _contexto.Clientes.Add(cliente);
        await _contexto.SaveChangesAsync();

        cliente.Telefono = "999000999";
        await _contexto.SaveChangesAsync();

        var fila = Auditorias()
            .FirstOrDefault(f => f.Operacion == "MODIFIED");

        Assert.NotNull(fila);

        using var doc = JsonDocument.Parse(fila.Diff!);

        var antes = doc.RootElement.GetProperty("antes");
        var despues = doc.RootElement.GetProperty("despues");

        Assert.Equal("999000222", antes.GetProperty("Telefono").GetString());
        Assert.Equal("999000999", despues.GetProperty("Telefono").GetString());
        Assert.False(antes.TryGetProperty("Nombres", out _));
    }

    [Fact]
    public async Task Eliminar_GeneraFilaDelete_ConAntes()
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            TipoDocumento = "DNI",
            NumeroDocumento = "80000003",
            Nombres = "Audit",
            Apellidos = "Tres",
            Telefono = "999000333",
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        };

        _contexto.Clientes.Add(cliente);
        await _contexto.SaveChangesAsync();

        _contexto.Clientes.Remove(cliente);
        await _contexto.SaveChangesAsync();

        var fila = Auditorias()
            .FirstOrDefault(f => f.Operacion == "DELETED");

        Assert.NotNull(fila);

        using var doc = JsonDocument.Parse(fila.Diff!);

        Assert.True(doc.RootElement
            .GetProperty("antes")
            .TryGetProperty("NumeroDocumento", out _));
    }

    [Fact]
    public async Task Secretos_SeEnmascaran()
    {
        _contexto.Usuarios.Add(new Usuario
        {
            Id = Guid.NewGuid(),
            Nombres = "Audit",
            Apellidos = "Cuatro",
            Email = "audit4@test.local",
            PasswordHash = "hash-secreto",
            Rol = "Cobrador",
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        });

        await _contexto.SaveChangesAsync();

        var fila = Auditorias()
            .FirstOrDefault(f => f.Tabla == "usuarios");

        Assert.NotNull(fila);
        Assert.DoesNotContain("hash-secreto", fila.Diff);
        Assert.Contains("***", fila.Diff);
    }

    [Fact]
    public async Task Auditoria_NoSeAutoAudita()
    {
        _contexto.Clientes.Add(new Cliente
        {
            Id = Guid.NewGuid(),
            TipoDocumento = "DNI",
            NumeroDocumento = "80000005",
            Nombres = "Audit",
            Apellidos = "Cinco",
            Telefono = "999000555",
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        });

        await _contexto.SaveChangesAsync();

        Assert.DoesNotContain(
            Auditorias(),
            f => f.Tabla == "auditoria");
    }
}
