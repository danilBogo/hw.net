using MongoDB.Bson;
using MongoDB.Driver;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Repositories;

public class FileRepository : IFileRepository
{
    public IMongoCollection<FileMetadata> FilesMetadata => _database.GetCollection<FileMetadata>(nameof(FileMetadata));
    private readonly IMongoDatabase _database;

    public FileRepository(IMongoDbConfiguration mongoDbConfiguration)
    {
        var client = new MongoClient(mongoDbConfiguration.ConnectionString);
        _database = client.GetDatabase(mongoDbConfiguration.Database);
    }

    public async Task<FileMetadata> GetFileWithFilter(string fileId)
    {
        var builder = new FilterDefinitionBuilder<FileMetadata>();
        var filter = builder.Empty;
        if (!string.IsNullOrWhiteSpace(fileId))
            filter &= builder.Regex("Id", new BsonRegularExpression(fileId));
        var currentCollection = _database.GetCollection<FileMetadata>(nameof(FileMetadata));
        return await currentCollection.Find(filter).SingleOrDefaultAsync();
    }


    public async Task<FileMetadata> CreateAsync(FileMetadata file)
    {
        await FilesMetadata.InsertOneAsync(file);
        return file;
    }
}