using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharedKernel.Files;

namespace SupportChat.Domain.Models.Files;

public class TxtFileMetadata : FileMetadata
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
}