using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SupportChat.Domain.Models.Files;

public class FileMetadata
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string Extension { get; set; } = null!;

    public string ContentType { get; set; } = null!;
    
    public long Size { get; set; }
}