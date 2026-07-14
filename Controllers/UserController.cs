using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/users
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var users = await _db.Users
            .AsNoTracking()
            .OrderBy(user => user.Id)
            .ToListAsync();

        return Ok(users);
    }

    // POST api/users/create
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
    
        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email
        };
    
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    
        return Ok(user);
    }
    
    // POST api/users
    [HttpPost]
    public async Task<ActionResult<User>> GetByEmail([FromBody] string email)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Email == email.Trim());
    
        if (user is null)
        {
            return NotFound();
        }
    
        return Ok(user);
    }
}
