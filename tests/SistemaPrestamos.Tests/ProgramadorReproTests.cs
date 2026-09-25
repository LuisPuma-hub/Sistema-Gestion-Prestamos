using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class ProgramadorReproTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;

    public ProgramadorReproTests()
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

    [Fact]
    public async Task ReglaMinutoActual_SeEjecutaYMarcaUlt()
    {
        var reglaRepo = new ReglaNotificacionRepository(_contexto);
        var envioRepo = new EnvioNotificacionRepository(_contexto);

        var reglas = new ReglaNotificacionService(
            reglaRepo, envioRepo, null!, null!);

        var lima = ProgramadorService.AhoraLima(DateTime.UtcNow);

        var creada = await reglas.CrearAsync(new CrearReglaDto
        {
            Nombre = "R",
            Evento = EventosNotificacion.ResumenDiario,
            Canal = CanalesNotificacion.Push,
            Hora = lima.ToString("HH:mm")
        });

        var programador = new ProgramadorService(
            reglaRepo,
            envioRepo,
            new PrestamoRepository(_contexto),
            new PeriodoInteresRepository(_contexto),
            new MorosidadRepository(_contexto),
            new PagoRepository(_contexto),
            new UsuarioRepository(_contexto),
            null!,
            new FakePush());

        await programador.EjecutarPendientesAsync(DateTime.UtcNow);

        var actual = await reglaRepo.ObtenerPorIdAsync(creada.Id);

        Assert.NotNull(actual!.UltimaEjecucion);
    }

    private class FakePush : SistemaPrestamos.Application.Interfaces.INotificacionService
    {
        public Task<int> EnviarAUsuarioAsync(
            Guid usuarioId, string titulo, string cuerpo)
        {
            return Task.FromResult(1);
        }

        public Task EnviarATokenAsync(
            string token, string titulo, string cuerpo)
        {
            return Task.CompletedTask;
        }
    }
}
