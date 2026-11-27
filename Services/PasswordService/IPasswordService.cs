using PasswordManager.Api.Models;

namespace PasswordManager.Services.PasswordService;

public interface IPasswordService
{
    public Task<(Vault?, string? ErrorCode, string? ErrorMessage)> CreateCurrentUserVault(CreatePassword vaultFields);
    public Task<(List<Vault>?, string? ErrorCode, string? ErrorMessage)> GetVaultByCurrentUserAsync();
    public Task<(bool?, string? ErrorCode, string? ErrorMessage)> DeletePasswordByCurrentUserAsync(string id);
    public Task<(Vault?, string? ErrorCode, string? ErrorMessage)> UpdatePasswordByCurrentUserAsync(UpdatePassword updateFields);
}