using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class MensajeWhatsappConfiguration : IEntityTypeConfiguration<MensajeWhatsapp>
{
    public void Configure(EntityTypeBuilder<MensajeWhatsapp> builder)
    {
        builder.ToTable("mensajes_whatsapp");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.TipoPlantilla)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NumeroDestino)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Contenido)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.IdentificadorExterno)
            .HasMaxLength(150);

        builder.Property(x => x.FechaCreacion)
            .IsRequired();
    }
}
