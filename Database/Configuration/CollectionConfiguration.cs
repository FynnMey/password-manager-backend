using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Api.Models;

public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> entity)
    {
        entity.ToTable("collection");
        
        entity.HasKey(vault => vault.Id);

        entity.Property(vault => vault.Name)
            .HasMaxLength(255);
    }
}