using MassTransit;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Models;
using SupportChat.Infrastructure.Services;

namespace SupportChat.RabbitMQListener.Consumers;

public class MessageConsumer : IConsumer<MessageFileMetadataDto>
{
    private readonly MessageService _messageService;

    public MessageConsumer(MessageService messageService)
    {
        _messageService = messageService;
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