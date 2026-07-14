using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Api.Models;

public class RefreshTokenConfiguration: IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.ToTable("refresh_token");

        entity.ToTable("refresh_token");

        entity.HasKey(rt => rt.Id);

        entity.Property(rt => rt.TokenHash)
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(rt => rt.ExpiresAt)
            .IsRequired();

        entity.Property(rt => rt.CreatedAt)
            .IsRequired();

        entity.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
