using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using SupportChat.Domain.Interfaces;

namespace SupportChat.Domain.Repositories;

public class FileRepository : IFileRepository
{
    private readonly IAmazonS3 _amazonS3;

    public FileRepository(IAmazonConfig amazonConfig)
    {
        var credentials = new BasicAWSCredentials(amazonConfig.AccessKey, amazonConfig.AccessSecret);
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ForcePathStyle = true,
            ServiceURL = amazonConfig.ServiceUrl,
        };
        _amazonS3 = new AmazonS3Client(credentials, config);
    }

    public async Task<string> SaveFileAsync(Stream stream, string bucketName, string? key = null)
    {
        var result = await TryCreateBucketIfNotExistsAsync(bucketName);
        if (!result) throw new Exception("Ошибка добавления");
        key ??= Guid.NewGuid().ToString();
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            InputStream = stream,
            Key = key
        };

        var response = await _amazonS3.PutObjectAsync(request);
        if ((int)response.HttpStatusCode < 300)
            return key;
        throw new Exception("Ошибка добавления");
    }

    public async Task<Stream?> DownloadFileAsync(string fileId, string bucketName)
    {
        var request = new GetObjectRequest
        {
            Key = fileId,
            BucketName = bucketName
        };
        var response = await _amazonS3.GetObjectAsync(request);

        if (response.HttpStatusCode is HttpStatusCode.NotFound || (int)response.HttpStatusCode >= 300)
            return null;

        return response.ResponseStream;
    }

    public async Task MoveFileToPersistantBucketAsync(string sourceBucket, string destinationBucket, string key)
    {
        var resultSourceBucket = await TryCreateBucketIfNotExistsAsync(sourceBucket);
        if (!resultSourceBucket) throw new Exception("Ошибка добавления");
        var resultDestinationBucket = await TryCreateBucketIfNotExistsAsync(destinationBucket);
        if (!resultDestinationBucket) throw new Exception("Ошибка добавления");
        await _amazonS3.CopyObjectAsync(sourceBucket, key, destinationBucket, key);
    }

    private async Task<bool> TryCreateBucketIfNotExistsAsync(string bucketName)
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
}