using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Api.Models;

public class AuthConfiguration: IEntityTypeConfiguration<Authentification>
{
    public void Configure(EntityTypeBuilder<Authentification> entity)
    {
        entity.ToTable("refresh_token");

        entity.HasKey(rt => rt.Id);

        entity.Property(rt => rt.RefreshTokenHash)
            .HasColumnName("TokenHash")
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(rt => rt.ExpiresAt)
            .IsRequired();

        entity.Property(rt => rt.CreatedAt)
            .IsRequired();

        entity.HasIndex(rt => rt.RefreshTokenHash)
            .IsUnique();
    }
}
