using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharedKernel.Files;

namespace SupportChat.Domain.Models.Files;

public class ImageFileMetadata : FileMetadata
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public int Width { get; set; }

    public int Height { get; set; }
}