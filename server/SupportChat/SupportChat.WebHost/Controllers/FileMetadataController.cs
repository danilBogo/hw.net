using Microsoft.AspNetCore.Mvc;
using SupportChat.Domain.Models.Files;
using SupportChat.Infrastructure.Services;

namespace SupportChat.WebHost.Controllers;

[ApiController]
[Route("[controller]")]
public class FileMetadataController : ControllerBase
{
    private readonly FileService _fileService;

    public FileMetadataController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<FileMetadata> File(string fileId)
    {
        var result = await _fileService.GetFileMetadata(fileId);
        return result;
    }
    
    [HttpPost]
    public async Task<FileMetadata> File(IFormFile file)
    {
        var fileMetadata = new FileMetadata
        {
            Name = file.FileName,
            Extension = Path.GetExtension(file.FileName),
            Size = file.Length,
            ContentType = file.ContentType
        };
        var fileId = await _fileService.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);
        fileMetadata.FileId = fileId.ToString();
        return await _fileService.CreateFileMetaData(fileMetadata);
    }
}