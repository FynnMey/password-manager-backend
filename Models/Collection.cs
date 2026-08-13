namespace PasswordManager.Api.Models;

public class Collection
{
    public int Id  { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
}