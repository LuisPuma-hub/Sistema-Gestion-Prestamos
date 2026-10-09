using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class FondoMovimientoConfiguration
    : IEntityTypeConfiguration<FondoMovimiento>
{
    public void Configure(EntityTypeBuilder<FondoMovimiento> builder)
    {
        builder.ToTable("fondo_movimientos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Tipo)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Monto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Fecha)
            .IsRequired();

        builder.Property(x => x.Motivo)
            .HasMaxLength(200);

        builder.Property(x => x.FechaRegistro)
            .IsRequired();
    }
}
