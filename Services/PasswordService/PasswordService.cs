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
}