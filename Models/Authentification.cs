namespace PasswordManager.Api.Models;

public class Authentification
{
    public string Id { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    
    public string RefreshTokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }
}
