using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Interfaces;

public interface IFileRepository
{
    Task<FileMetadata> GetFileWithFilter(string messageId);

    Task<FileMetadata> CreateAsync(FileMetadata newFile);
}