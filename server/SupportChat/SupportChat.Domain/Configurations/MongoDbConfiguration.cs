using SupportChat.Domain.Interfaces;

namespace SupportChat.Domain.Configurations;

public class MongoDbConfiguration : IMongoDbConfiguration
{
    public string Database { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
}