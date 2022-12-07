namespace SupportChat.Domain.Interfaces;

public interface IAmazonConfig
{
    public string ServiceUrl { get; set; }
    
    public string AccessKey { get; set; }
    
    public string AccessSecret { get; set; }
}