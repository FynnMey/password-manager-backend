using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Api.Models;

public class VaultConfiguration : IEntityTypeConfiguration<Vault>
{
    public void Configure(EntityTypeBuilder<Vault> entity)
    {
        entity.ToTable("vault");
        
        entity.HasKey(vault => vault.Id);

        entity.Property(vault => vault.EncryptedName)
            .HasMaxLength(255);

        entity.Property(vault => vault.EncryptedEmail)
            .HasMaxLength(255);

        entity.Property(vault => vault.EncryptedPassword)
            .HasMaxLength(255);

        entity.Property(vault => vault.EncryptedWebsite)
            .HasMaxLength(255);

        entity.Property(vault => vault.EncryptedNote)
            .HasColumnType("text");
    }
}