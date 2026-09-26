using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

namespace SistemaPrestamos.Tests;

public class NotificacionesTests : IDisposable
{
    private readonly PrestamosDbContext _contexto;
    private readonly ReglaNotificacionService _reglas;
    private readonly ProgramadorService _programador;

    public NotificacionesTests()
    {
        var opciones = new DbContextOptionsBuilder<PrestamosDbContext>()
            .UseInMemoryDatabase($"test_{Guid.NewGuid()}")
            .Options;

        _contexto = new PrestamosDbContext(opciones);

        var reglaRepo = new ReglaNotificacionRepository(_contexto);
        var envioRepo = new EnvioNotificacionRepository(_contexto);

        _reglas = new ReglaNotificacionService(
            reglaRepo,
            envioRepo,
            null!,
            null!);

        _programador = new ProgramadorService(
            reglaRepo,
            envioRepo,
            new PrestamoRepository(_contexto),
            new PeriodoInteresRepository(_contexto),
            new MorosidadRepository(_contexto),
            new PagoRepository(_contexto),
            new UsuarioRepository(_contexto),
            null!,
            null!);
    }

    public void Dispose()
    {
        _contexto.Dispose();
    }

    [Fact]
    public async Task Crear_HoraInvalida_Lanza()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reglas.CrearAsync(new CrearReglaDto
            {
                Nombre = "X",
                Evento = EventosNotificacion.VenceHoy,
                Canal = CanalesNotificacion.Whatsapp,
                Hora = "25:00"
            }));
    }

    [Fact]
    public async Task Crear_EventoInvalido_Lanza()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reglas.CrearAsync(new CrearReglaDto
            {
                Nombre = "X",
                Evento = "NoExiste",
                Canal = CanalesNotificacion.Whatsapp,
                Hora = "08:00"
            }));
    }

    [Fact]
    public async Task Crear_PushEventoNoResumen_Lanza()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reglas.CrearAsync(new CrearReglaDto
            {
                Nombre = "X",
                Evento = EventosNotificacion.VenceHoy,
                Canal = CanalesNotificacion.Push,
                Hora = "08:00"
            }));
    }

    [Fact]
    public async Task Crear_Valida_AsignaPlantillaDefecto()
    {
        var regla = await _reglas.CrearAsync(new CrearReglaDto
        {
            Nombre = "Vencen hoy",
            Evento = EventosNotificacion.VenceHoy,
            Canal = CanalesNotificacion.Whatsapp,
            Hora = "08:00"
        });

        Assert.Equal("recordatorio_pago_v2", regla.Plantilla);
        Assert.True(regla.Activa);
        Assert.Equal(127, regla.DiasSemana);
    }

    [Fact]
    public void TocaAhora_VentanaQuinceMinutos()
    {
        var regla = new ReglaNotificacion
        {
            Hora = new TimeOnly(8, 0)
        };

        Assert.True(ProgramadorService.TocaAhora(
            regla, new DateTime(2026, 9, 24, 8, 0, 0)));
        Assert.True(ProgramadorService.TocaAhora(
            regla, new DateTime(2026, 9, 24, 8, 14, 0)));
        Assert.False(ProgramadorService.TocaAhora(
            regla, new DateTime(2026, 9, 24, 8, 16, 0)));
        Assert.False(ProgramadorService.TocaAhora(
            regla, new DateTime(2026, 9, 24, 7, 59, 0)));
    }

    [Fact]
    public async Task Crear_PushMoraCobrador_Valido()
    {
        var regla = await _reglas.CrearAsync(new CrearReglaDto
        {
            Nombre = "Mora al cobrador",
            Evento = EventosNotificacion.MoraCobrador,
            Canal = CanalesNotificacion.Push,
            Hora = "08:00"
        });

        Assert.Equal(EventosNotificacion.MoraCobrador, regla.Evento);
    }

    [Fact]
    public async Task Crear_WhatsappMoraCobrador_Rechazado()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reglas.CrearAsync(new CrearReglaDto
            {
                Nombre = "X",
                Evento = EventosNotificacion.MoraCobrador,
                Canal = CanalesNotificacion.Whatsapp,
                Hora = "08:00"
            }));
    }

    [Fact]
    public async Task Crear_PushResumenDiario_Valido()
    {
        var regla = await _reglas.CrearAsync(new CrearReglaDto
        {
            Nombre = "Resumen diario",
            Evento = EventosNotificacion.ResumenDiario,
            Canal = CanalesNotificacion.Push,
            Hora = "07:30"
        });

        Assert.Null(regla.Plantilla);
    }

    [Fact]
    public void TocaHoy_BitmaskRespetaDia()
    {
        var regla = new ReglaNotificacion { DiasSemana = 1 }; // solo lunes

        var lunes = new DateTime(2026, 9, 21, 8, 0, 0); // lunes
        var martes = new DateTime(2026, 9, 22, 8, 0, 0);

        Assert.True(ProgramadorService.TocaHoy(regla, lunes));
        Assert.False(ProgramadorService.TocaHoy(regla, martes));
    }

    [Fact]
    public void AhoraLima_EsUtcMenosCinco()
    {
        var utc = new DateTime(2026, 9, 24, 13, 0, 0, DateTimeKind.Utc);

        var lima = ProgramadorService.AhoraLima(utc);

        Assert.Equal(8, lima.Hour);
    }

    [Fact]
    public async Task Ejecutar_UnaSolaVezPorMinuto()
    {
        var lima = ProgramadorService.AhoraLima(DateTime.UtcNow);

        await _reglas.CrearAsync(new CrearReglaDto
        {
            Nombre = "Resumen",
            Evento = EventosNotificacion.ResumenDiario,
            Canal = CanalesNotificacion.Push,
            Hora = lima.ToString("HH:mm")
        });

        var primera = await _programador
            .EjecutarPendientesAsync(DateTime.UtcNow);

        var segunda = await _programador
            .EjecutarPendientesAsync(DateTime.UtcNow);

        Assert.Equal(0, primera); // sin cobradores: 0 envíos
        Assert.Equal(0, segunda); // UltimaEjecucion bloquea repetición
    }

    [Fact]
    public async Task ExisteHoy_DeNocheLima_CuentaInstanteUtc()
    {
        var regla = await _reglas.CrearAsync(new CrearReglaDto
        {
            Nombre = "Nocturna",
            Evento = EventosNotificacion.VenceHoy,
            Canal = CanalesNotificacion.Whatsapp,
            Hora = "21:30"
        });

        var repo = new EnvioNotificacionRepository(_contexto);
        var clienteId = Guid.NewGuid();
        var prestamoId = Guid.NewGuid();

        // 21:30 en Lima = 02:30 UTC del día siguiente.
        var hoyLima = new DateTime(2026, 9, 25);

        await repo.CrearAsync(new EnvioNotificacion
        {
            Id = Guid.NewGuid(),
            ReglaId = regla.Id,
            Evento = regla.Evento,
            Canal = regla.Canal,
            ClienteId = clienteId,
            PrestamoId = prestamoId,
            Destinatario = "Test",
            Estado = "Enviado",
            FechaCreacion = new DateTime(
                2026, 9, 26, 2, 30, 0, DateTimeKind.Utc)
        });

        await repo.GuardarCambiosAsync();

        Assert.True(await repo.ExisteHoyAsync(
            regla.Id, regla.Evento, regla.Canal,
            clienteId, prestamoId, null, hoyLima));
    }

    [Fact]
    public async Task ExisteHoy_EvitaDuplicados()
    {
        var regla = await _reglas.CrearAsync(new CrearReglaDto
        {
            Nombre = "Vencen hoy",
            Evento = EventosNotificacion.VenceHoy,
            Canal = CanalesNotificacion.Whatsapp,
            Hora = "08:00"
        });

        var repo = new EnvioNotificacionRepository(_contexto);
        var clienteId = Guid.NewGuid();
        var prestamoId = Guid.NewGuid();
        var hoy = ProgramadorService.AhoraLima(DateTime.UtcNow).Date;

        await repo.CrearAsync(new EnvioNotificacion
        {
            Id = Guid.NewGuid(),
            ReglaId = regla.Id,
            Evento = regla.Evento,
            Canal = regla.Canal,
            ClienteId = clienteId,
            PrestamoId = prestamoId,
            Destinatario = "Test",
            Estado = "Enviado",
            FechaCreacion = DateTime.UtcNow
        });

        await repo.GuardarCambiosAsync();

        Assert.True(await repo.ExisteHoyAsync(
            regla.Id, regla.Evento, regla.Canal,
            clienteId, prestamoId, null, hoy));

        Assert.False(await repo.ExisteHoyAsync(
            regla.Id, regla.Evento, regla.Canal,
            Guid.NewGuid(), prestamoId, null, hoy));
    }
}
