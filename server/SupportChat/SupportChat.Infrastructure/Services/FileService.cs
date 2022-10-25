using System.Text.Json;
using SharedKernel.Files;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models.Files;
using SupportChat.Infrastructure.Classes.Files;
using File = SupportChat.Infrastructure.Classes.Files.File;

namespace SupportChat.Infrastructure.Services;

public class FileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<IEnumerable<ImageFileMetadata>> GetImageFileMetaData(Guid messageId) =>
        await GetFileMetadata<ImageFileMetadata>(messageId);

    public async Task<IEnumerable<TxtFileMetadata>> GetTxtFileMetaData(Guid messageId) =>
        await GetFileMetadata<TxtFileMetadata>(messageId);

    public async Task CreateFileMetaData<T>(T newFile) where T : FileMetadata =>
        await _fileRepository.CreateAsync(newFile);

    public dynamic GetFile(string json, string fileExtension)
    {
        return fileExtension switch
        {
            "png" => ParseFile<ImageFile>(json),
            "txt" => ParseFile<TxtFile>(json),
            _ => throw new Exception("Can`t be parsed")
        };
    }

    private T ParseFile<T>(string json) where T : File
    {
        var file = JsonSerializer.Deserialize<T>(json);
        return file ?? throw new Exception("Can`t be parsed");
    }

    private async Task<IEnumerable<T>> GetFileMetadata<T>(Guid messageId) where T : FileMetadata =>
        await _fileRepository.GetCollectionWithFilter<T>(messageId);
}