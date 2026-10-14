using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class PrestamoConfiguration : IEntityTypeConfiguration<Prestamo>
{
    public void Configure(EntityTypeBuilder<Prestamo> builder)
    {
        builder.ToTable("prestamos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CapitalInicial)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TasaInteresSemanal)
            .HasPrecision(10, 6)
            .IsRequired();

        builder.Property(x => x.EsquemaInteres)
            .HasMaxLength(10)
            .HasDefaultValue("Saldo")
            .IsRequired();

        builder.Property(x => x.ModalidadCobro)
            .HasMaxLength(10)
            .HasDefaultValue("Semanal")
            .IsRequired();

        builder.Property(x => x.FrecuenciaCuota)
            .HasMaxLength(10)
            .HasDefaultValue("Diaria")
            .IsRequired();

        builder.Property(x => x.PlazoCuotas);

        builder.Property(x => x.FechaFin);

        builder.Property(x => x.MontoTotal)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(x => x.CuotaFija)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(x => x.CapitalPendiente)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.FechaInicio)
            .IsRequired();

        builder.Property(x => x.FechaAprobacion);

        builder.Property(x => x.Estado)
            .HasMaxLength(30)
            .IsRequired();

        // Cliente → Préstamos
        builder.HasOne(x => x.Cliente)
            .WithMany(x => x.Prestamos)
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Garante → Préstamo
        builder.HasOne(x => x.Garante)
            .WithMany()
            .HasForeignKey(x => x.GaranteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}