using MassTransit;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Models;
using SupportChat.Infrastructure.Services;

namespace SupportChat.RabbitMQListener.Consumers;

public class MessageConsumer : IConsumer<MessageFileMetadataDto>
{
    private readonly MessageService _messageService;
    private readonly FileService _fileService;

    public MessageConsumer(MessageService messageService, FileService fileService)
    {
        _messageService = messageService;
        _fileService = fileService;
    }

    public async Task Consume(ConsumeContext<MessageFileMetadataDto> context)
    {
        var message = new Message
        {
            Content = context.Message.Content,
            Time = context.Message.Time,
            FileId = context.Message.FileMetadata.Id
        };
        await _messageService.AddMessageAsync(message);
    }
}