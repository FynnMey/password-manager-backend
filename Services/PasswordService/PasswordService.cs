using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Database;
using PasswordManager.Api.Models;
using PasswordManager.Api.Services;
using PasswordManager.Services.CurrentUserService;

namespace PasswordManager.Services.PasswordService;

public class PasswordService(AppDbContext db, IUuidGenerator uuidGenerator, ICurrentUserService currentUserService) : IPasswordService
{
    public async Task<(Vault?, string? ErrorCode, string? ErrorMessage)> CreateCurrentUserVault(CreatePassword vaultFields)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return (null, "UNAUTHORIZED", "Unauthorized User");
        
        var vault = new Vault
        {
            Id = uuidGenerator.NewUuid(),
            UserId = userId, 
            
            EncryptedName = vaultFields.EncryptedName,
            EncryptedEmail = vaultFields.EncryptedEmail,
            EncryptedPassword = vaultFields.EncryptedPassword,
            EncryptedWebsite = vaultFields.EncryptedWebsite,
            EncryptedNote = vaultFields.EncryptedNote,
        };
        
        db.Vaults.Add(vault);
        await db.SaveChangesAsync();
        
        return (vault, null, null);
    }

    public async Task<(List<Vault>?, string? ErrorCode, string? ErrorMessage)> GetVaultByCurrentUserAsync()
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return (null, "UNAUTHORIZED", "Unauthorized User");
        
        var userVault = await db.Vaults
            .AsNoTracking()
            .Where(vault => vault.UserId == userId)
            .ToListAsync();
        
        return (userVault, null, null);
    }
    
    public async Task<(bool?, string? ErrorCode, string? ErrorMessage)> DeletePasswordByCurrentUserAsync(string id)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return (null, "UNAUTHORIZED", "Unauthorized User");
        
        var deletedCount = await db.Vaults
            .Where(vault => vault.Id == id && vault.UserId == userId)
            .ExecuteDeleteAsync();

        if (deletedCount == 0)
            return (null, "NOT_FOUND", "The specified password does not exist.");

        return (true, null, null);
    }

    public async Task<(Vault?, string? ErrorCode, string? ErrorMessage)> UpdatePasswordByCurrentUserAsync(UpdatePassword updateFields)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return (null, "UNAUTHORIZED", "Unauthorized User");
        
        var existingVault = await db.Vaults
            .FirstOrDefaultAsync(vault => vault.Id == updateFields.Id && vault.UserId == userId);

        if (existingVault is null)
            return (null, "Not Found", "Vault not found");

        existingVault.EncryptedName = updateFields.EncryptedName ?? existingVault.EncryptedName;
        existingVault.EncryptedEmail = updateFields.EncryptedEmail ?? existingVault.EncryptedEmail;
        existingVault.EncryptedPassword = updateFields.EncryptedPassword ?? existingVault.EncryptedPassword;
        existingVault.EncryptedWebsite = updateFields.EncryptedWebsite ?? existingVault.EncryptedWebsite;
        existingVault.EncryptedNote = updateFields.EncryptedNote ?? existingVault.EncryptedNote;
    
        existingVault.EditAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        
        return (existingVault, null, null);
    }
}