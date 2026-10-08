using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models;

public class UpdatePassword
{
    [Required]
    public string Id { get; set; } = string.Empty;
    
    public string? EncryptedName { get; set; }
    
    [MaxLength(255)]
    public string? EncryptedEmail { get; set; }
    
    [MaxLength(255)]
    public string? EncryptedPassword { get; set; }
    
    [MaxLength(255)]
    public string? EncryptedWebsite { get; set; }
    
    public string? EncryptedNote { get; set; }
    
    public int? CollectionId { get; set; }
}