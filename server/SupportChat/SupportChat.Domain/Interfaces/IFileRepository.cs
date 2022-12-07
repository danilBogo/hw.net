namespace SupportChat.Domain.Interfaces;

public interface IFileRepository
{
    Task<string> SaveFileAsync(Stream stream, string bucketName, string? key = null);

    Task<Stream?> DownloadFileAsync(string fileId, string bucketName);

    Task MoveFileToPersistantBucketAsync(string sourceBucket, string destinationBucket, string key);
}