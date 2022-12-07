using System.Diagnostics;
using System.Text.Json;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SupportChat.Domain.Constants;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Enums;
using SupportChat.Domain.Models.Files;
using SupportChat.Infrastructure.Services;
using SupportChat.RabbitMQListener.SignalR;

namespace SupportChat.RabbitMQListener.Consumers;

public class FileUploadConsumer : IConsumer<FileUploadDto>
{
    private readonly CacheService _cacheService;
    private readonly FileService _fileService;
    private readonly MetadataService _metadataService;
    private readonly IHubContext<FileUploadedHub> _hub;

    public FileUploadConsumer(CacheService cacheService, FileService fileService, MetadataService metadataService,
        IHubContext<FileUploadedHub> hub)
    {
        _cacheService = cacheService;
        _fileService = fileService;
        _metadataService = metadataService;
        _hub = hub;
    }

    public async Task Consume(ConsumeContext<FileUploadDto> context)
    {
        var requestIdKey = context.Message.RequestId;
        await _cacheService.Incr(requestIdKey);
        var value = await _cacheService.GetByHeaderKeyAndRequestId(RedisHeaderRecord.Counter, requestIdKey);
        if (long.TryParse(value, out var longValue) && longValue == 2)
        {
            var fileId = await _cacheService.GetByHeaderKeyAndRequestId(RedisHeaderRecord.FileId, requestIdKey);
            await _fileService.MoveFileToPersistantBucketAsync(AmazonConstants.TempBucketName,
                AmazonConstants.PersistentBucketName, fileId);
            var json = await _cacheService.GetByHeaderKeyAndRequestId(RedisHeaderRecord.Metadata, requestIdKey);
            var metadata = JsonSerializer.Deserialize<Metadata>(json);
            if (metadata is null)
                throw new Exception("Can not deserialize metadata");
            metadata.FileId = fileId;
            await _metadataService.CreateMetadataAsync(metadata);
            await _hub.Clients.Client(context.Message.UserId).SendAsync("ReceiveMessage", "Файл был успешно загружен", fileId);
        }
    }
}