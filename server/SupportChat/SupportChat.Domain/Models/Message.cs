using SharedKernel.BaseEntities;

namespace SupportChat.Domain.Models;

public class Message : BaseEntity
{
    public string Content { get; set; } = null!;
    
    public DateTime Time { get; set; }
}