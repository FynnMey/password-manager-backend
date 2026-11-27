using PasswordManager.Api.Models;

namespace PasswordManager.Services.PasswordService;

public interface IPasswordService
{
    public Task<(Vault?, string? ErrorCode, string? ErrorMessage)> CreateCurrentUserVault(CreatePassword vaultFields);
}