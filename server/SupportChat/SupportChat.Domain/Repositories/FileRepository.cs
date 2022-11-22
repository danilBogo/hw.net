using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using MongoDB.Driver;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Repositories;

public class FileRepository : IFileRepository
{
    private readonly IAmazonS3 _amazonS3;
    public IMongoCollection<FileMetadata> FilesMetadata => _database.GetCollection<FileMetadata>(nameof(FileMetadata));
    private readonly IMongoDatabase _database;
    private readonly string _tempBucketName = "tempbucket";
    private readonly string _persistentBucketName = "persistentbucket";

    public FileRepository(IMongoDbConfiguration mongoDbConfiguration, IAmazonS3 amazonS3)
    {
        var client = new MongoClient(mongoDbConfiguration.ConnectionString);
        _database = client.GetDatabase(mongoDbConfiguration.Database);
        _amazonS3 = amazonS3;
    }

    public async Task<FileMetadata> GetFileMetadataWithFilter(string fileId) =>
        await FilesMetadata.Find(f => f.Id == fileId).FirstOrDefaultAsync();


    public async Task<FileMetadata> CreateFileMetadataAsync(FileMetadata file)
    {
        await FilesMetadata.InsertOneAsync(file);
        return file;
    }

    public async Task<Guid> SaveFileAsync(Stream stream, string fileName, string contentType)
    {
        var result = await IsBucketExistsAsync(_tempBucketName);
        if (!result) throw new Exception("Ошибка добавления");
        var id = Guid.NewGuid();
        var request = new PutObjectRequest
        {
            BucketName = _tempBucketName,
            InputStream = stream,
            Key = id.ToString(),
            ContentType = contentType,
        };
        
        var response = await _amazonS3.PutObjectAsync(request);
        if ((int)response.HttpStatusCode < 300) 
            return id;
        throw new Exception("Ошибка добавления");
    }

    private async Task<bool> IsBucketExistsAsync(string bucketName)
    {
        var bucketExists = await _amazonS3.DoesS3BucketExistAsync(bucketName);
        if (bucketExists)
            return true;
        await CreateBucketAsync(bucketName);
        return true;
    }

    private async Task CreateBucketAsync(string bucketName)
    {
        var request = new PutBucketRequest
        {
            BucketName = bucketName
        };

        await _amazonS3.PutBucketAsync(request);
    }
    
    public async Task<Stream?> DownloadFileAsync(Guid fileId)
    {
        var request = new GetObjectRequest
        {
            Key = fileId.ToString(),
            BucketName = _tempBucketName
        };
        var response = await _amazonS3.GetObjectAsync(request);
        
        if (response.HttpStatusCode is HttpStatusCode.NotFound || (int)response.HttpStatusCode >= 300)
            return null;
        
        return response.ResponseStream;
    }
 
}