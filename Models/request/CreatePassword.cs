using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models;

public class CreatePassword
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string Website { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string Note { get; set; } = string.Empty;
    
    public int CollectionId { get; set; }
}