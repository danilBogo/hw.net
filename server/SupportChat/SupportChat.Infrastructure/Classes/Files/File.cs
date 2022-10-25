namespace SupportChat.Infrastructure.Classes.Files;

public abstract class File
{
    public string Name { get; set; } = null!;
    
    public string Extension { get; set; } = null!;
    
    public ulong Size { get; set; }
}