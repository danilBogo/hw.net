namespace SupportChat.Domain.Interfaces;

public interface IMongoDbConfiguration
{
    public string Database { get; set; }
    public string ConnectionString { get; }
}