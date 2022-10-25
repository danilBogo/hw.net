using Microsoft.AspNetCore.Http;

namespace SupportChat.Domain.Dto;

public class MessageDto
{
    public string Content { get; set; } = null!;
    
    public DateTime Time { get; set; }

    public IFormFile FormFile { get; set; } = null!;

    public string JsonMetadata { get; set; } = null!;
}