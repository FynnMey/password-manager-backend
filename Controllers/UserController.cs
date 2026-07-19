using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    // POST api/user/create
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<User>> Create(CreateUserRequest request)
    {
        var email = request.Email.Trim();
        var emailExists = await _db.Users
            .AnyAsync(user => user.Email == email);
    
        if (emailExists)
        {
            return Conflict(new { message = "A user with this email already exists." });
        }
        
        
        var passwordHash = _hasher.HashPassword(null, request.Password.Trim());
    
        var user = new User
        {
            Name = request.Name.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash = passwordHash,
			IsPremium = request.IsPremium,
			IsAdmin = request.IsAdmin,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow
        };
    
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    
        return Ok(user);
    }    
}
