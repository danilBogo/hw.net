using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Interfaces;

public interface IFileRepository
{
    Task<FileMetadata> GetFileMetadataWithFilter(string messageId);

    Task<FileMetadata> CreateFileMetadataAsync(FileMetadata newFile);

    Task<Guid> SaveFileAsync(Stream stream, string fileName, string contentType);

    Task<Stream?> DownloadFileAsync(Guid fileId);
}