using Passwordmanager.Application.Interfaces;
using Passwordmanager.Domain;
using Microsoft.EntityFrameworkCore;

namespace Passwordmanager.Application.Users.CreateUser;

public sealed class CreateUserHandler (IAppDbContext db)
{
    public async Task<bool> HandleAsync (CreateUserCommand command, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == command.Username, ct);
        if (user is not null)
        {
            Console.WriteLine($"Hey, der User {command.Username} existiert bereits");
            return false;
        }
        
        var hash = BCrypt.Net.BCrypt.HashPassword(command.Password, 13);
        
        var newUser = User.Create(command.Username, hash);
        
        await db.Users.AddAsync(newUser, ct);
        await db.SaveChangesAsync(ct);
        
        Console.WriteLine($"Hey user {command.Username} angelegt");
        return true;
    }
}
