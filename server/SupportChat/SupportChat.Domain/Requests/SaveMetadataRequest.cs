using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Requests;

public class SaveMetadataRequest
{
    public string RequestId { get; set; } = default!;
    
    public Metadata Metadata { get; set; } = default!;
    
    public string UserId { get; set; } = default!;
    
}