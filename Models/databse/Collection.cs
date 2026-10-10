namespace PasswordManager.Api.Models;

public class Collection
{
    public int Id  { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
}