using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Models;

namespace PasswordManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    public DbSet<User> Users => Set<User>();
	public DbSet<Authentification> RefreshToken => Set<Authentification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
 		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
