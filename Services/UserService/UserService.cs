using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Database;
using PasswordManager.Api.Models;
using PasswordManager.Services.CurrentUserService;
using PasswordManager.Services.PasswordHasherService;

namespace PasswordManager.Services.UserService;

public class UserService(AppDbContext db, IPasswordHasherService passwordHasherService, ICurrentUserService currentUserService) : IUserService
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

    public bool UpdateSaltAndCarnary(RegisterUser request)
    {
        var userId = CurrentUserId();
        if (userId is null)
            return false;
        
        var updatedRows = db.Users.Where(user => user.Id == userId)
            .ExecuteUpdate(b => b
                .SetProperty(user => user.Salt, request.Salt)
                .SetProperty(user => user.CanaryValue, request.CanaryValue)
            );

        return updatedRows > 0;
    }
    
    public async Task<(User? User, string? ErrorCode, string? ErrorMessage)> GetUserByIdAsync()
    {
        if (currentUserService.UserId is null)
            return (null, "UNAUTHORIZED", "Unauthorized User");
        var userId = CurrentUserId();
        var user = await db.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if (user is null)
            return (null, "USER_NOT_FOUND", "The specified user does not exist.");
        
        return (user, null, null);
    }

    private string? CurrentUserId()
    {
        return currentUserService.UserId;
    }
}