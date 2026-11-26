using PasswordManager.Api.Models;
using PasswordManager.Api.Models.premium;

namespace PasswordManager.Services.PremiumService;

public interface IPremiumService
{
    public Task<(bool?, string? ErrorCode, string? ErrorMessage)> ActivatePremiumCode(ActivatePremium request);
}