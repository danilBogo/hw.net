using SupportChat.Domain.Interfaces;

namespace SupportChat.Domain.Configuration;

public class MongoDbConfiguration : IMongoDbConfiguration
{
    public string Database { get; set; } = default!;

    public string ConnectionString { get; set; } = default!;
}