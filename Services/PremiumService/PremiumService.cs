using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Database;
using PasswordManager.Api.Models;
using PasswordManager.Api.Models.premium;
using PasswordManager.Services.UserService;

namespace PasswordManager.Services.PremiumService;

public class PremiumService(AppDbContext db, IUserService userService) : IPremiumService
{
    public async Task<(bool?, string? ErrorCode, string? ErrorMessage)> ActivatePremiumCode(ActivatePremium request)
    {
        var (user, userErrorCode, userErrorMessage) = await userService.GetUserByIdAsync();
        if (user is null)
            return (null, userErrorCode!, userErrorMessage!);
        
        var (premiumCode, errorCode, errorMessage) = await ValidatePremiumCode(request.Code);
        if (premiumCode is null)
            return (null, errorCode!, errorMessage!);
            
        user.PremiumUntil = premiumCode.DurationInDays is null 
            ? DateTime.MaxValue 
            : DateTime.UtcNow.AddDays(premiumCode.DurationInDays.Value);
        
        premiumCode.RedeemedByUserId = user.Id;
        premiumCode.RedeemedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        
        return (true, null, null);
    }

    private async Task<(PremiumCode? premiumCode, string? ErrorCode, string? ErrorMessage)> ValidatePremiumCode(string premiumCode)
    {
        var result = await db.PremiumCodes.FirstOrDefaultAsync(code => code.Code == premiumCode);
        
        if (result is null)
            return (null, "INVALID_CODE", "The specified code is invalid.");
        
        if (result.IsUsed)
            return (null, "CODE_ALREADY_USED", "The specified code has already been used.");
        
        return (result, null, null);
    }
}