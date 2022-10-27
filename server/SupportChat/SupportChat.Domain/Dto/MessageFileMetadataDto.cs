using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Dto;

public class MessageFileMetadataDto
{
    public string Content { get; set; } = null!;
    
    public DateTime Time { get; set; }

    public FileMetadata FileMetadata { get; set; } = null!;
}