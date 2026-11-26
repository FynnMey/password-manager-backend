using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Api.Common;
using PasswordManager.Api.Controllers;
using PasswordManager.Api.Database;
using PasswordManager.Api.Models;
using PasswordManager.Api.Models.premium;
using PasswordManager.Api.Services;
using PasswordManager.Services.PremiumService;
using PasswordManager.Services.UserService;

namespace PasswordManager.Controllers;

[ApiController]
[Route("api/premium")]
public class PremiumController(AppDbContext db, IUuidGenerator uuidGenerator, IPremiumService premiumService) : BaseApiController
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
        
        var (isSuccess, errorCode, errorMessage) = await premiumService.ActivatePremiumCode(request);
        
        if (isSuccess == true)
            return Success(true);

        return Failure<bool>(400, errorCode!, errorMessage!);
    }
    
    private string? GetUserId()
    {
        return User.FindFirstValue((ClaimTypes.NameIdentifier));
    }
}