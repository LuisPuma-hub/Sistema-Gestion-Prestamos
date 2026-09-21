using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.ToTable("auditoria");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Tabla)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.IdRegistro)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Operacion)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Fecha)
            .IsRequired();
    }
}
