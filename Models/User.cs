namespace PasswordManager.Api.Models;

public class User
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    
    public string Salt { get; set; } = string.Empty;
    
    public string CanaryValue { get; set; } = string.Empty;

    public bool IsPremium { get; set; } = false;

    public bool IsAdmin { get; set; } = false;

    public int FailedLoginAttempts { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
