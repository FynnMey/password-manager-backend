namespace PasswordManager.Api.Models;

public class Vault
{
    public int Id  { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public string EncryptedName { get; set; } = string.Empty;
    
    public string EncryptedEmail { get; set; } = string.Empty;
    
    public string EncryptedPassword { get; set; } = string.Empty;
    
    public string EncryptedWebsite { get; set; } = string.Empty;
    
    public string EncryptedNote { get; set; } = string.Empty;
    
    public int? CollectionId { get; set; }
    public Collection? Collection { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime EditAt { get; set; }
}