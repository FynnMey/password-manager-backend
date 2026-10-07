namespace PasswordManager.Services.PasswordHasherService;

public interface IPasswordHasherService
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}