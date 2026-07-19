namespace PasswordManager.Api.Models;

public class Authentification
{
    public int Id { get; set; }

    public int UserId { get; set; }
    
    public string RefreshTokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }
}
