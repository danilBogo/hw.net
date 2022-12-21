using SharedKernel.BaseEntities;

namespace SupportChat.Domain.Models;

public class Message : BaseEntity
{
    public string Content { get; set; } = null!;
    
    public string UserName { get; set; } = null!;
    
    public string InterlocutorName { get; set; } = null!;
    
    public DateTime Time { get; set; }

    public string FileId { get; set; } = null!;
}