using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models;

public class RegisterUser
{
    [Required]
    [MaxLength(255)]
    public string CanaryValue { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string Salt { get; set; } = string.Empty;
}