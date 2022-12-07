using SupportChat.Domain.Interfaces;

namespace SupportChat.Infrastructure.Services;

public class FileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }
    
    public async Task<string> SaveFileAsync(Stream stream, string fileName, string? key = null) =>
        await _fileRepository.SaveFileAsync(stream, fileName, key);

    public async Task<Stream?> DownloadFileAsync(string fileId, string bucketName) =>
        await _fileRepository.DownloadFileAsync(fileId, bucketName);

    public async Task MoveFileToPersistantBucketAsync(string sourceBucket, string destinationBucket, string key) =>
        await _fileRepository.MoveFileToPersistantBucketAsync(sourceBucket, destinationBucket, key);
}