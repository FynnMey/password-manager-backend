using PasswordManager.Api.Models;

namespace PasswordManager.Services.UserService;

public interface IUserService
{
    Task<(User? User, string? ErrorCode, string? ErrorMessage)> CreateUserAsync(CreateUser request);
    bool UpdateSaltAndCarnary(RegisterUser request);
    public Task<(User? User, string? ErrorCode, string? ErrorMessage)> GetUserByIdAsync();

}