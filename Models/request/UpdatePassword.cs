using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models;

public class UpdatePassword
{
    [Required]
    public string Id { get; set; } = string.Empty;
    
    public string EncryptedName { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string EncryptedEmail { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string EncryptedPassword { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string EncryptedWebsite { get; set; } = string.Empty;
    
    public string EncryptedNote { get; set; } = string.Empty;
    
    public int CollectionId { get; set; }
}