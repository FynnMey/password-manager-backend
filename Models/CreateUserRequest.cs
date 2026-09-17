using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models;

public class CreateUserRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    public bool IsPremium { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
}
