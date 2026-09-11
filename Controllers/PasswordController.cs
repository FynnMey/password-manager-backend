using System.Security.Claims;
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
    public async Task<ActionResult<Vault>> Create(CreatePassword request)
    {
        var  userId = User.FindFirstValue((ClaimTypes.NameIdentifier));
        
        if (string.IsNullOrEmpty(userId))
            return BadRequest();
        
        var vault = new Vault
        {
            UserId = userId, 
            
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Website = request.Website,
            Note = request.Note,
        };
        
        _db.Vaults.Add(vault);
        await _db.SaveChangesAsync();
        
        return Ok(vault);
    }
    
    [Authorize]
    [HttpPost("get-all")]
    public async Task<ActionResult<Vault>> GetAllFromUser()
    {
        var  userId = User.FindFirstValue((ClaimTypes.NameIdentifier));
        
        if (string.IsNullOrEmpty(userId))
            return BadRequest();
        
        var userVault = _db.Vaults.Where(vault => vault.UserId == userId);
        
        return Ok(userVault);
    }
}