using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Api.Common;
using PasswordManager.Api.Controllers;
using PasswordManager.Api.Data;
using PasswordManager.Api.Models;
using PasswordManager.Api.Models.premium;
using PasswordManager.Api.Services;

namespace PasswordManager.Controllers;

[ApiController]
[Route("api/premium")]
public class PremiumController(AppDbContext db, IUuidGenerator uuidGenerator) : BaseApiController
{
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public ActionResult<ApiResponse<PremiumCode>> Create(CreatePremium request)
    {
        if (request is { IsLifetime: false, DurationInDays: null })
            return Failure<PremiumCode>(400, "INVALID_DURATION", "The duration is missing or invalid.");
        
        var newCode = new PremiumCode
        {
            Code = uuidGenerator.NewUuid(),
            DurationInDays = request.IsLifetime ? null : request.DurationInDays,
            CreatedAt = DateTime.UtcNow
        };
        
        db.PremiumCodes.Add(newCode);
        db.SaveChanges();
        
        return Success(newCode);
    }

    [Authorize]
    [HttpPost("activate")]
    public async Task<ActionResult<ApiResponse<bool>>> Activate(ActivatePremium request)
    {
        var userId = GetUserId();
        
        if (userId is null)
            return UnauthorizedUser<bool>();

        var premiumCode = await db.PremiumCodes.FirstOrDefaultAsync(code => code.Code == request.Code);
        
        if (premiumCode is null || premiumCode.IsUsed)
            return Success(false);

        var user = await db.Users.FirstOrDefaultAsync(user => user.Id == userId);
        
        if (user is null)
            return Failure<bool>(400, "USER_NOT_FOUND", "The specified user does not exist.");
            
        user.PremiumUntil = premiumCode.DurationInDays is null 
            ? DateTime.MaxValue 
            : DateTime.UtcNow.AddDays(premiumCode.DurationInDays.Value);
        
        premiumCode.RedeemedByUserId = userId;
        premiumCode.RedeemedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        
        return Success(true);
    }
    
    private string? GetUserId()
    {
        return User.FindFirstValue((ClaimTypes.NameIdentifier));
    }
}