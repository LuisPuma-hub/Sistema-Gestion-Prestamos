using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class NotaCompromisoConfiguration
    : IEntityTypeConfiguration<NotaCompromiso>
{
    public void Configure(EntityTypeBuilder<NotaCompromiso> builder)
    {
        builder.ToTable("notas_compromiso");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Texto)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.FechaCompromiso)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasMaxLength(20)
            .HasDefaultValue("Pendiente")
            .IsRequired();

        builder.Property(x => x.FechaCreacion)
            .IsRequired();

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Prestamo)
            .WithMany()
            .HasForeignKey(x => x.PrestamoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
