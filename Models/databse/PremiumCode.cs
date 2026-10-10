using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PasswordManager.Api.Models;

public class PremiumCode
{
    [Key]
    public string Code { get; set; } = string.Empty;
    
    public int? DurationInDays { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RedeemedAt { get; set; }
    
    [JsonIgnore]
    public User? RedeemedByUser { get; set; }
    public string? RedeemedByUserId { get; set; }

    public bool IsUsed => RedeemedAt.HasValue;
}