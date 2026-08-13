using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Api.Models;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("users");

        entity.HasKey(user => user.Id);

        entity.Property(user => user.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(user => user.LastName)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(user => user.Email)
            .HasMaxLength(255)
            .IsRequired();

        entity.HasIndex(user => user.Email)
            .IsUnique();

        entity.Property(user => user.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(user => user.IsPremium)
            .IsRequired();

        entity.Property(user => user.IsAdmin)
            .IsRequired();

        entity.Property(user => user.FailedLoginAttempts)
            .IsRequired();

        entity.Property(user => user.CreatedAt)
            .IsRequired();
    }
}
