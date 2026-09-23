using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Common;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/user/password")]
public class PasswordController(AppDbContext db) : BaseApiController
{
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<Vault>>> Create(CreatePassword request)
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
            return UnauthorizedUser<Vault>();
        
        var vault = new Vault
        {
            UserId = userId, 
            
            EncryptedName = request.EncryptedName,
            EncryptedEmail = request.EncryptedEmail,
            EncryptedPassword = request.EncryptedPassword,
            EncryptedWebsite = request.EncryptedWebsite,
            EncryptedNote = request.EncryptedNote,
        };
        
        db.Vaults.Add(vault);
        await db.SaveChangesAsync();
        
        return Success(vault);
    }
    
    [Authorize]
    [HttpPost("get-all")]
    public async Task<ActionResult<ApiResponse<List<Vault>>>> GetAllFromUser()
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
            return UnauthorizedUser<List<Vault>>();
        
        var userVault = await db.Vaults
            .AsNoTracking()
            .Where(vault => vault.UserId == userId)
            .ToListAsync();
        
        return Success(userVault);
    }
    
    private string? GetUserId()
    {
        return User.FindFirstValue((ClaimTypes.NameIdentifier));
    }
}