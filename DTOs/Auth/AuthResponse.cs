namespace PasswordManager.DTOs.Auth;

public record AuthResponse(
    string AccessToken,
    UserDto User
);

public record UserDto(
    string Id,
    string Name,
    string LastName,
    string Email,
    bool IsPremium,
    bool IsAdmin,
    string CanaryValue,
    string Salt
);