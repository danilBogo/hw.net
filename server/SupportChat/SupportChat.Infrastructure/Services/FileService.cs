using System.Text;
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
        await _fileRepository.GetFileWithFilter(fileId);

    public async Task<FileMetadata> CreateFileMetaData(FileMetadata newFile) =>
        await _fileRepository.CreateAsync(newFile);
}