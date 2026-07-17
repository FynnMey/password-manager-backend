using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Api.Models;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();
    
    public AuthController(AppDbContext db)
    {
        _db = db;
    }
    
    
    private bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
    
    // POST api/auth
    [HttpPost]
    public async Task<ActionResult<User>> GetByEmail(GetUserRequest request)
    {
        var email = request.Email.Trim();
        var passwrord = request.Password.Trim();
        
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Email == email);
        
    
        if (user is null)
            return NotFound();
        
        if (this.VerifyPassword(user.PasswordHash, passwrord) == false)
            return Unauthorized(new { message = "Invalid password." });
        
        
    
        return Ok(user);
    }
}