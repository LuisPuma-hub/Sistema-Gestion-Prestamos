using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("pagos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Monto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.MontoInteres)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.MontoCapital)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.FechaPago)
            .IsRequired();

        builder.Property(x => x.Comprobante)
            .HasMaxLength(50);

        builder.Property(x => x.Observaciones)
            .HasMaxLength(500);

        builder.Property(x => x.FechaRegistro)
            .IsRequired();

        // Préstamo → Pagos
        builder.HasOne(x => x.Prestamo)
            .WithMany(x => x.Pagos)
            .HasForeignKey(x => x.PrestamoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
