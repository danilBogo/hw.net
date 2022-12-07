using System.Text.Json;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Enums;
using SupportChat.Domain.Models.Files;
using SupportChat.Domain.Requests;
using SupportChat.Infrastructure.Services;

namespace SupportChat.WebHost.Controllers;

[ApiController]
[Route("[controller]")]
public class MetadataController : ControllerBase
{
    private readonly MetadataService _metadataService;
    private readonly IBus _bus;
    private readonly CacheService _cacheService;

    public MetadataController(MetadataService metadataService, IBus bus, CacheService cacheService)
    {
        _metadataService = metadataService;
        _bus = bus;
        _cacheService = cacheService;
    }

    [HttpGet]
    public async Task<Metadata> Metadata(string fileId)
    {
        var result = await _metadataService.GetMetadataByFileIdAsync(fileId);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Metadata(SaveMetadataRequest request)
    {
        var json = JsonSerializer.Serialize(request.Metadata);
        await _cacheService.Add(RedisHeaderRecord.Metadata, request.RequestId, json);
        await _bus.Publish(new FileUploadDto
        {
            UserId = request.UserId,
            RequestId = request.RequestId
        });
        return Ok();
    }
}