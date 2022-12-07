using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SupportChat.Domain.Models.Files;

public class Metadata
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public string Value { get; set; } = null!;
    public string FileId { get; set; } = null!;
}