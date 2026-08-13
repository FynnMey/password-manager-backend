using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/user/password")]
public class PasswordController : ControllerBase
{
    private readonly AppDbContext _db;
    
    public PasswordController(AppDbContext db)
    {
        _db = db;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<Boolean>> Create(CreatePassword request)
    {
        var vault = new Vault
        {
            UserId = request.UserId, 
            
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Website = request.Website,
            Note = request.Note,
        };
        
        _db.Vaults.Add(vault);
        await _db.SaveChangesAsync();
        
        
        return true;
    }
}