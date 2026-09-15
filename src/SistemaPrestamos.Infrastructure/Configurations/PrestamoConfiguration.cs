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