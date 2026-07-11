namespace PasswordManager.Api.Models;

public class RefreshToken
{
    public int Id { get; set; }

    public int BenutzerId { get; set; }
    public Benutzer Benutzer { get; set; } = null!;

    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }
}