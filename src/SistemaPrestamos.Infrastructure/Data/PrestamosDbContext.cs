using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Data;

public class PrestamosDbContext : DbContext
{
    public PrestamosDbContext(DbContextOptions<PrestamosDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Garante> Garantes => Set<Garante>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<Morosidad> Morosidades => Set<Morosidad>();
    public DbSet<PeriodoInteres> PeriodosInteres => Set<PeriodoInteres>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PrestamosDbContext).Assembly);
    }
}