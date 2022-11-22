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
    private readonly FileService _fileService;

    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult?> File(string fileId, string contentType, string name)
    {
        var stream = await _fileService.DownloadFileAsync(new Guid(fileId));
        if (stream is null)
        return BadRequest();
        return File(stream, contentType, name);
    }
}