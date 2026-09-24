using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class ReglaNotificacionConfiguration : IEntityTypeConfiguration<ReglaNotificacion>
{
    public void Configure(EntityTypeBuilder<ReglaNotificacion> builder)
    {
        builder.ToTable("reglas_notificacion");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Nombre)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.Evento)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(x => x.Canal)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Hora)
            .IsRequired();

        builder.Property(x => x.Plantilla)
            .HasMaxLength(100);

        builder.Property(x => x.FechaCreacion)
            .IsRequired();
    }
}
