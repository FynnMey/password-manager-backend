using DefaultNamespace;
using Microsoft.EntityFrameworkCore;

namespace Passwordmanager.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}