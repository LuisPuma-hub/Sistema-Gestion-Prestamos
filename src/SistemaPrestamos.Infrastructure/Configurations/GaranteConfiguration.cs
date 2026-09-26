using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class GaranteConfiguration : IEntityTypeConfiguration<Garante>
{
    public void Configure(EntityTypeBuilder<Garante> builder)
    {
        builder.ToTable("garantes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Nombres)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Apellidos)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Telefono)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Direccion)
            .HasMaxLength(200);

        builder.Property(x => x.FechaRegistro)
            .IsRequired();

        // Cliente → Garantes (opcional)
        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
