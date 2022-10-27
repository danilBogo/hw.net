using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SupportChat.Domain.Models;
using SupportChat.Domain.Models.Files;
using SupportChat.Infrastructure.Services;

namespace SupportChat.WebHost.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly MessageService _messageService;
    private readonly FileService _fileService;

    public FileController(MessageService messageService, FileService fileService)
    {
        _messageService = messageService;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<FileMetadata> File(string fileId) => 
        await _fileService.GetFileMetadata(fileId);

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
        return await _fileService.CreateFileMetaData(fileMetadata);
    }
}