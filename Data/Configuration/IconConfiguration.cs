using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Api.Models;

public class IconConfiguration : IEntityTypeConfiguration<Icon>
{
    public void Configure(EntityTypeBuilder<Icon> entity)
    {
        entity.ToTable("icons");

        entity.HasKey(icon => icon.Url);

        entity.Property(user => user.Url)
            .IsRequired();

        entity.Property(user => user.Image)
            .IsRequired();
    }
}