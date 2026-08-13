namespace PasswordManager.Api.Models;

public class Vault
{
    public int Id  { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string Website { get; set; } = string.Empty;
    
    public string Note { get; set; } = string.Empty;
    
    public int? CollectionId { get; set; }
    public Collection? Collection { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime EditAt { get; set; }
}