using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Models;

namespace PasswordManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Vault> Vaults => Set<Vault>();
    public DbSet<Collection> Collections => Set<Collection>();
	public DbSet<Authentification> RefreshToken => Set<Authentification>();
	public DbSet<Icon> Icons => Set<Icon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
 		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
