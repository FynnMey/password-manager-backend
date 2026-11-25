using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PasswordManager.Api.Models;

public class PremiumConfiguration : IEntityTypeConfiguration<PremiumCode>
{
    public void Configure(EntityTypeBuilder<PremiumCode> entity)
    {
        entity.ToTable("premium_code");

        entity.HasKey(pc => pc.Code);
        
        entity.Property(rt => rt.Code)
            .IsRequired()
            .ValueGeneratedNever();

        entity.Property(rt => rt.DurationInDays)
            .IsRequired(false);
        
        entity.Property(rt => rt.CreatedAt)
            .IsRequired();

        entity.Property(rt => rt.RedeemedAt)
            .IsRequired(false); 
        
        entity.Property(rt => rt.RedeemedByUserId)
            .IsRequired(false);

        entity.Ignore(rt => rt.IsUsed);

        entity.HasOne(pc => pc.RedeemedByUser)
            .WithMany()
            .HasForeignKey(pc => pc.RedeemedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}