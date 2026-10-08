using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Common;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using PasswordManager.Api.Services;

namespace PasswordManager.Api.Controllers;

[ApiController]
[Route("api/user/password")]
public class PasswordController(AppDbContext db, IUuidGenerator uuidGenerator) : BaseApiController
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
            Id = uuidGenerator.NewUuid(),
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
    [HttpPost("delete")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(DeletePassword request)
    {
        var userId = GetUserId();
    
        var deletedCount = await db.Vaults
            .Where(vault => vault.Id == request.Id && vault.UserId == userId)
            .ExecuteDeleteAsync();

        if (deletedCount == 0)
            return UnauthorizedUser<bool>(); 
        
        return Success(true);
    }
    
    [Authorize]
    [HttpPost("update")]
    public async Task<ActionResult<ApiResponse<Vault>>> Update(UpdatePassword request)
    {
        var userId = GetUserId();
    
        var existingVault = await db.Vaults
            .FirstOrDefaultAsync(vault => vault.Id == request.Id && vault.UserId == userId);

        if (existingVault is null)
            return Failure<Vault>(404, "Not Found", "Vault not found");

        existingVault.EncryptedName = request.EncryptedName ?? existingVault.EncryptedName;
        existingVault.EncryptedEmail = request.EncryptedEmail ?? existingVault.EncryptedEmail;
        existingVault.EncryptedPassword = request.EncryptedPassword ?? existingVault.EncryptedPassword;
        existingVault.EncryptedWebsite = request.EncryptedWebsite ?? existingVault.EncryptedWebsite;
        existingVault.EncryptedNote = request.EncryptedNote ?? existingVault.EncryptedNote;
    
        existingVault.EditAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
    
        return Success(existingVault);
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