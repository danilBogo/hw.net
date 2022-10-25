using MongoDB.Bson;
using MongoDB.Driver;
using SharedKernel.Files;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Repositories;

public class FileRepository : IFileRepository
{
    public IMongoCollection<ImageFileMetadata> ImageFilesMetadata { get; } = null!;
    public IMongoCollection<TxtFileMetadata> TxtFilesMetadata { get; } = null!;
    private readonly IMongoDatabase _database;
    
    public FileRepository(IMongoDbConfiguration mongoDbConfiguration)
    {
        var client = new MongoClient(mongoDbConfiguration.ConnectionString);
        _database = client.GetDatabase(mongoDbConfiguration.Database);
    }
    
    private IMongoCollection<T> GetCollection<T>(string name = nameof(T)) where T : FileMetadata =>
        _database.GetCollection<T>(name);
    
    public async Task<IEnumerable<T>> GetCollectionWithFilter<T>(Guid messageId) where T : FileMetadata
    {
        var builder = new FilterDefinitionBuilder<T>();
        var filter = builder.Empty;
        var str = messageId.ToString();
        if (!string.IsNullOrWhiteSpace(str))
            filter &= builder.Regex("MessageId", new BsonRegularExpression(str));
        var currentCollection = GetCollection<T>();
        return await currentCollection.Find(filter).ToListAsync();
    }
}