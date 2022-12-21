using MongoDB.Driver;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Repositories;

public class MetadataRepository : IMetadataRepository
{
    private IMongoCollection<Metadata> Metadata => _database.GetCollection<Metadata>(nameof(Models.Files.Metadata));
    private readonly IMongoDatabase _database;

    public MetadataRepository(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task<Metadata> GetMetadataByFileIdAsync(string fileId) =>
        await Metadata.Find(f => f.FileId == fileId).FirstOrDefaultAsync();


    public async Task<Metadata> CreateMetadataAsync(Metadata metadata)
    {
        await Metadata.InsertOneAsync(metadata);
        return metadata;
    }
}