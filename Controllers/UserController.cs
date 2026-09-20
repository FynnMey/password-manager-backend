using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(AppDbContext db) : ControllerBase
{
    private readonly PasswordHasher<User> _hasher = new();
    
    // POST api/user/register
    [Authorize]
    [HttpPost("register")]
    public ActionResult Register(RegisterUser request)
    {
        var  userId = User.FindFirstValue((ClaimTypes.NameIdentifier));
        
        if (string.IsNullOrWhiteSpace(request.CanaryValue))
            return BadRequest(new { message = "Invalid canary value." });
        
        if (string.IsNullOrWhiteSpace(request.Salt))
            return BadRequest(new { message = "Invalid salt." });

        db.Users.Where(user => user.Id == userId)
            .ExecuteUpdate<User>(b => b
                .SetProperty(user => user.Salt, request.Salt)
                .SetProperty(user => user.CanaryValue, request.CanaryValue)
            );

        return Ok();
    }

    // POST api/user/create
    [HttpPost("create")]
    public async Task<ActionResult<User>> Create(CreateUserRequest request)
    {
        var email = request.Email.Trim();
        var emailExists = await db.Users
            .AnyAsync(user => user.Email == email);
    
        if (emailExists)
        {
            return Conflict(new { message = "A user with this email already exists." });
        }
        
        
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
    
        return Ok(user);
    }  
}
