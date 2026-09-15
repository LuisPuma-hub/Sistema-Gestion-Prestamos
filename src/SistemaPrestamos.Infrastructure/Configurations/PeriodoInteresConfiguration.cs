using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class PeriodoInteresConfiguration
    : IEntityTypeConfiguration<PeriodoInteres>
{
    public void Configure(EntityTypeBuilder<PeriodoInteres> builder)
    {
        builder.ToTable("periodos_interes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.FechaInicio)
            .IsRequired();

        builder.Property(x => x.FechaVencimiento)
            .IsRequired();

        builder.Property(x => x.InteresGenerado)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.InteresPagado)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.InteresPendiente)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.FechaPagoCompleto);

        builder.Property(x => x.FechaRegistro)
            .IsRequired();

        builder.HasOne(x => x.Prestamo)
            .WithMany(x => x.PeriodosInteres)
            .HasForeignKey(x => x.PrestamoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.PrestamoId,
            x.FechaInicio
        })
        .IsUnique();
    }
}