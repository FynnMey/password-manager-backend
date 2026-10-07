using PasswordManager.Api.Models;

namespace PasswordManager.Services.TokenService;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string HashRefreshToken(string token);
}