using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using PasswordManager.Services.PasswordHasherService;
using PasswordManager.Services.UserService;

namespace password;

public class UserService(AppDbContext db, IPasswordHasherService passwordHasherService) : IUserService
{
    public async Task<(User? User, string? ErrorCode, string? ErrorMessage)> CreateUserAsync(CreateUser request)
    {
        var email = request.Email.Trim();
        var emailExists = await db.Users.AnyAsync(user => user.Email == email);

        if (emailExists)
            return (null, "EMAIL_ALREADY_EXISTS", "The specified email address already exists.");

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash = passwordHasherService.Hash(request.Password.Trim()),
            PremiumUntil = request.PremiumUntil,
            IsAdmin = request.IsAdmin,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return (user, null, null);
    }
}