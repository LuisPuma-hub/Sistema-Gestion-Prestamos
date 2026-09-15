using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class MorosidadConfiguration : IEntityTypeConfiguration<Morosidad>
{
    public void Configure(EntityTypeBuilder<Morosidad> builder)
    {
        builder.ToTable("morosidades");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.PagosInteresVencidos)
            .IsRequired();

        builder.Property(x => x.FechaInicio);

        builder.Property(x => x.FechaReactivacion);

        builder.Property(x => x.Activa)
            .IsRequired();

        builder.Property(x => x.Observaciones)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Prestamo)
            .WithMany()
            .HasForeignKey(x => x.PrestamoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.PrestamoId)
            .IsUnique();
    }
}