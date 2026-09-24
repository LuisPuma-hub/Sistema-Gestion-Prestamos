using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Data;

public class PrestamosDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public PrestamosDbContext(
        DbContextOptions<PrestamosDbContext> options,
        IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Garante> Garantes => Set<Garante>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<Morosidad> Morosidades => Set<Morosidad>();
    public DbSet<Dispositivo> Dispositivos => Set<Dispositivo>();
    public DbSet<MensajeWhatsapp> MensajesWhatsapp => Set<MensajeWhatsapp>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();
    public DbSet<PeriodoInteres> PeriodosInteres => Set<PeriodoInteres>();
    public DbSet<ReglaNotificacion> ReglasNotificacion => Set<ReglaNotificacion>();
    public DbSet<EnvioNotificacion> EnviosNotificacion => Set<EnvioNotificacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PrestamosDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var auditorias = GenerarAuditorias();

        var resultado = await base.SaveChangesAsync(cancellationToken);

        if (auditorias.Count > 0)
        {
            await Auditorias.AddRangeAsync(auditorias, cancellationToken);
            await base.SaveChangesAsync(cancellationToken);
        }

        return resultado;
    }

    private List<Auditoria> GenerarAuditorias()
    {
        var lista = new List<Auditoria>();

        var entradas = ChangeTracker.Entries()
            .Where(e =>
                e.State is EntityState.Added
                    or EntityState.Modified
                    or EntityState.Deleted
                && e.Entity is not Auditoria)
            .ToList();

        if (entradas.Count == 0)
        {
            return lista;
        }

        var idUsuario = _httpContextAccessor?.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        foreach (var entrada in entradas)
        {
            lista.Add(new Auditoria
            {
                Id = Guid.NewGuid(),
                Tabla = entrada.Metadata.GetTableName() ?? entrada.Entity.GetType().Name,
                IdRegistro = ObtenerClave(entrada),
                Operacion = entrada.State.ToString().ToUpperInvariant(),
                Diff = SerializarDiff(entrada),
                UsuarioId = Guid.TryParse(idUsuario, out var uid) ? uid : null,
                Fecha = DateTime.UtcNow
            });
        }

        return lista;
    }

    private static string ObtenerClave(EntityEntry entrada)
    {
        var clave = entrada.Properties
            .Where(p => p.Metadata.IsPrimaryKey())
            .Select(p => p.CurrentValue?.ToString() ?? "-");

        return string.Join(",", clave);
    }

    private static string SerializarDiff(EntityEntry entrada)
    {
        var antes = new Dictionary<string, object?>();
        var despues = new Dictionary<string, object?>();

        foreach (var propiedad in entrada.Properties)
        {
            var nombre = propiedad.Metadata.Name;

            switch (entrada.State)
            {
                case EntityState.Added:
                    despues[nombre] = ValorSeguro(propiedad.CurrentValue, nombre);
                    break;

                case EntityState.Deleted:
                    antes[nombre] = ValorSeguro(propiedad.OriginalValue, nombre);
                    break;

                case EntityState.Modified:
                    if (propiedad.IsModified)
                    {
                        antes[nombre] = ValorSeguro(propiedad.OriginalValue, nombre);
                        despues[nombre] = ValorSeguro(propiedad.CurrentValue, nombre);
                    }
                    break;
            }
        }

        return JsonSerializer.Serialize(new
        {
            antes,
            despues
        });
    }

    private static object? ValorSeguro(object? valor, string nombre)
    {
        // Nunca auditar secretos.
        if (nombre.Contains("Password", StringComparison.OrdinalIgnoreCase) ||
            nombre.Contains("Token", StringComparison.OrdinalIgnoreCase))
        {
            return "***";
        }

        return valor;
    }
}
