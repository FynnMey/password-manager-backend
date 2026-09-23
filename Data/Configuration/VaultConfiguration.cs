using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Api.Models;

public class VaultConfiguration : IEntityTypeConfiguration<Vault>
{
    public void Configure(EntityTypeBuilder<Vault> entity)
    {
        entity.ToTable("vault");
        
        entity.HasKey(vault => vault.Id);

        entity.Property(vault => vault.Name)
            .HasMaxLength(255);

        entity.Property(vault => vault.Email)
            .HasMaxLength(255);

        entity.Property(vault => vault.Password)
            .HasMaxLength(255);

        entity.Property(vault => vault.Website)
            .HasMaxLength(255);

        entity.Property(vault => vault.Note)
            .HasColumnType("text");
    }
}