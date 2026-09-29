using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Infrastructure.Configurations;

public class PasswordResetConfiguration : IEntityTypeConfiguration<PasswordReset>
{
    public void Configure(EntityTypeBuilder<PasswordReset> builder)
    {
        builder.ToTable("password_resets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Token)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.Property(x => x.ExpiraEn)
            .IsRequired();

        builder.Property(x => x.Usado)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.FechaCreacion)
            .IsRequired();

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
