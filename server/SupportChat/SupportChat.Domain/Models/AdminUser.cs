namespace SupportChat.Domain.Models;

public sealed class AdminUser
{
    public string? AdminId { get; set; }
    
    public string? AdminName { get; set; }

    public string? UserId { get; set; }
    
    public string? UserName { get; set; }
}