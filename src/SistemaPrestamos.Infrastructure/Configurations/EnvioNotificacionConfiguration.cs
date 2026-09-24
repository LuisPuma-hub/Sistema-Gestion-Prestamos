using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class EnvioNotificacionConfiguration : IEntityTypeConfiguration<EnvioNotificacion>
{
    public void Configure(EntityTypeBuilder<EnvioNotificacion> builder)
    {
        builder.ToTable("envios_notificacion");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Evento)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(x => x.Canal)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Destinatario)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.FechaCreacion)
            .IsRequired();

        builder.HasOne(x => x.Regla)
            .WithMany()
            .HasForeignKey(x => x.ReglaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
