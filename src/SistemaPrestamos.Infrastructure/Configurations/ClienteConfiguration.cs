using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.TipoDocumento)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.NumeroDocumento)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(x => x.NumeroDocumento)
            .IsUnique();

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
            .HasMaxLength(255);

        builder.Property(x => x.ReferenciaDireccion)
            .HasMaxLength(255);

        builder.Property(x => x.FotoReciboServicio)
            .HasMaxLength(500);

        builder.Property(x => x.Observaciones)
            .HasMaxLength(1000);

        builder.Property(x => x.Estado)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.FechaRegistro)
            .IsRequired();
    }
}