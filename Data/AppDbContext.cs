using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Models;

namespace PasswordManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    public DbSet<Benutzer> Benutzer => Set<Benutzer>();
	public DbSet<RefreshToken> RefreshToken => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Benutzer>(entity =>
        {
            entity.ToTable("benutzer");

            entity.HasKey(benutzer => benutzer.Id);

            entity.Property(benutzer => benutzer.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(benutzer => benutzer.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(benutzer => benutzer.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(benutzer => benutzer.Email)
                .IsUnique();

            entity.Property(benutzer => benutzer.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(benutzer => benutzer.EncryptedVault)
                .IsRequired(false);

            entity.Property(benutzer => benutzer.IsPremium)
                .IsRequired();

            entity.Property(benutzer => benutzer.IsAdmin)
                .IsRequired();

            entity.Property(benutzer => benutzer.FailedLoginAttempts)
                .IsRequired();

            entity.Property(benutzer => benutzer.CreatedAt)
                .IsRequired();
        });

		modelBuilder.Entity<RefreshToken>(entity =>
    {
        entity.ToTable("refresh_token");

        entity.HasKey(rt => rt.Id);

        entity.Property(rt => rt.TokenHash)
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(rt => rt.ExpiresAt)
            .IsRequired();

        entity.Property(rt => rt.CreatedAt)
            .IsRequired();

        entity.HasOne(rt => rt.Benutzer)
            .WithMany()
            .HasForeignKey(rt => rt.BenutzerId)
            .OnDelete(DeleteBehavior.Cascade);
    });
    }
}