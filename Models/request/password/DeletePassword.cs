using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models;

public class DeletePassword
{
    [Required]
    public string Id { get; set; } = string.Empty;
}