using Microsoft.AspNetCore.Http;

namespace SupportChat.Domain.Requests;

public sealed class SaveFileRequest
{
    public string RequestId { get; set; } = default!;
    
    public string UserId { get; set; } = default!;
    
    public IFormFile File { get; set; } = default!;
}