using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models.Files;

namespace SupportChat.Infrastructure.Services;

public class FileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<FileMetadata> GetFileMetadata(string fileId) =>
        await _fileRepository.GetFileMetadataWithFilter(fileId);

    public async Task<FileMetadata> CreateFileMetaData(FileMetadata newFile) =>
        await _fileRepository.CreateFileMetadataAsync(newFile);

    public async Task<Guid> SaveFileAsync(Stream stream, string fileName, string contentType) =>
        await _fileRepository.SaveFileAsync(stream, fileName, contentType);

    public async Task<Stream?> DownloadFileAsync(Guid fileId) =>
        await _fileRepository.DownloadFileAsync(fileId);
}