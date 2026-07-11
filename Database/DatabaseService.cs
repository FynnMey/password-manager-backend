using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;

namespace PasswordManager.Api.Database;

public static class DatabaseService
{
    public static async Task EnsureDatabaseCreatedAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.EnsureCreatedAsync();
    }
}
