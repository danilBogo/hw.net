using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SupportChat.Domain.Constants;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Enums;
using SupportChat.Domain.Requests;
using SupportChat.Infrastructure.Services;
using SupportChat.WebHost.SignalR;

namespace SupportChat.WebHost.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly IBus _bus;
    private readonly FileService _fileService;
    private readonly CacheService _cacheService;

    public FileController(IBus bus, FileService fileService, CacheService cacheService)
    {
        _bus = bus;
        _fileService = fileService;
        _cacheService = cacheService;
    }

    [HttpGet]
    public new async Task<IActionResult> File(string fileId, string contentType, string name)
    {
        var stream = await _fileService.DownloadFileAsync(fileId, AmazonConstants.PersistentBucketName);
        if (stream is null)
            return BadRequest();
        return File(stream, contentType, name);
    }

    [HttpPost]
    public async Task<IActionResult> File([FromForm] SaveFileRequest request)
    {
        var fileId = await _fileService.SaveFileAsync(request.File.OpenReadStream(), AmazonConstants.TempBucketName);
        await _cacheService.Add(RedisHeaderRecord.FileId, request.RequestId, fileId);
        await _bus.Publish(new FileUploadDto
        {
            UserId = request.UserId,
            RequestId = request.RequestId
        });
        return Ok();
    }
}