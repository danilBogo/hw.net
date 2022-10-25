namespace SharedKernel.Files;

public class FileMetadata
{
    public Guid MessageId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Extension { get; set; } = null!;
    
    public ulong Size { get; set; }
}