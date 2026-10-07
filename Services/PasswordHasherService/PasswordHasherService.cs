using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Services.PasswordHasherService;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<object> _hasher = new();
    private static readonly object DummyUser = new();
    
    public string Hash(string password)
    {
        return _hasher.HashPassword(DummyUser, password);
    }
    
    public bool Verify(string password, string passwordHash)
    {
        var result = _hasher.VerifyHashedPassword(DummyUser, passwordHash, password);
        return result is PasswordVerificationResult.Success 
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}