using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Dto;

public class MessageMetadataDto
{
    public string Content { get; set; } = null!;

    public string UserName { get; set; } = null!;
    
    public string InterlocutorName { get; set; } = null!;

    public DateTime Time { get; set; }

    public Metadata Metadata { get; set; } = null!;
}