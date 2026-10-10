using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Api.Models.premium;

public class CreatePremium
{
    [Range(1, 3650)] 
    public int? DurationInDays { get; set; }

    public bool IsLifetime { get; set; } = false;
}