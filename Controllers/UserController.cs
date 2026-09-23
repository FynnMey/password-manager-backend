using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using Microsoft.AspNetCore.Identity;
using PasswordManager.Api.Common;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(AppDbContext db) : BaseApiController
{
    private readonly PasswordHasher<User> _hasher = new();
    
    // POST api/user/register
    [Authorize]
    [HttpPost("register")]
    public ActionResult<ApiResponse<bool>> Register(RegisterUser request)
    {
        var  userId = User.FindFirstValue((ClaimTypes.NameIdentifier));
        
        if (string.IsNullOrWhiteSpace(request.CanaryValue))
            return Failure<bool>(400, "INVALID_CANARY_VALUE", "The canary value is missing or invalid.");
        
        if (string.IsNullOrWhiteSpace(request.Salt))
            return Failure<bool>(400, "INVALID_SALT", "The salt is missing or invalid.");

        db.Users.Where(user => user.Id == userId)
            .ExecuteUpdate(b => b
                .SetProperty(user => user.Salt, request.Salt)
                .SetProperty(user => user.CanaryValue, request.CanaryValue)
            );

        return Success(true);
    }

    // POST api/user/create
    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<User>>> Create(CreateUserRequest request)
    {
        var email = request.Email.Trim();
        var emailExists = await db.Users
            .AnyAsync(user => user.Email == email);

        if (emailExists)
            return Failure<User>(400, "EMAIL_NOT_FOUND", "The specified email address does not exist.");
        
        var passwordHash = _hasher.HashPassword(new User(), request.Password.Trim());
    
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash = passwordHash,
			IsPremium = request.IsPremium,
			IsAdmin = request.IsAdmin,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow
        };
    
        db.Users.Add(user);
        await db.SaveChangesAsync();
    
        return Success(user);
    }
}
