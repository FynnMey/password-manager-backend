using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models;

public class GetIconRequest
{
    [Required]
    public string Url { get; set; } = string.Empty;
}