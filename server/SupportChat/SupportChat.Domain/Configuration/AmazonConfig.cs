using SupportChat.Domain.Interfaces;

namespace SupportChat.Domain.Configuration;

public class AmazonConfig : IAmazonConfig
{
    public string ServiceUrl { get; set; } = default!;
    
    public string AccessKey { get; set; } = default!;
    
    public string AccessSecret { get; set; } = default!;
}