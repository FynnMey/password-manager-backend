using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BenutzerController : ControllerBase
{
    private readonly AppDbContext _db;

    public BenutzerController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/benutzer
    [HttpGet]
    public async Task<ActionResult<List<Benutzer>>> GetAll()
    {
        var benutzer = await _db.Benutzer
            .AsNoTracking()
            .OrderBy(benutzer => benutzer.Id)
            .ToListAsync();

        return Ok(benutzer);
    }

    // POST api/benutzer/create
    [HttpPost("create")]
    public async Task<ActionResult<Benutzer>> Create(CreateBenutzerRequest request)
    {
        var email = request.Email.Trim();
        var emailExists = await _db.Benutzer
            .AnyAsync(benutzer => benutzer.Email == email);
    
        if (emailExists)
        {
            return Conflict(new { message = "Ein Benutzer mit dieser Email existiert bereits." });
        }
    
        var benutzer = new Benutzer
        {
            Name = request.Name.Trim(),
            Email = email
        };
    
        _db.Benutzer.Add(benutzer);
        await _db.SaveChangesAsync();
    
        return Ok(benutzer);
    }
    
    // POST api/benutzer
    [HttpPost]
    public async Task<ActionResult<Benutzer>> GetByEmail([FromBody] string email)
    {
        var benutzer = await _db.Benutzer
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Email == email.Trim());
    
        if (benutzer is null)
        {
            return NotFound();
        }
    
        return Ok(benutzer);
    }
}
