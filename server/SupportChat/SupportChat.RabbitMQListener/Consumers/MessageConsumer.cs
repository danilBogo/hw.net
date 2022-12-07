using MassTransit;
using SupportChat.Domain.Constants;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Enums;
using SupportChat.Domain.Models;
using SupportChat.Infrastructure.Services;

namespace SupportChat.RabbitMQListener.Consumers;

public class MessageConsumer : IConsumer<MessageMetadataDto>
{
    private readonly MessageService _messageService;

    public MessageConsumer(MessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task Consume(ConsumeContext<MessageMetadataDto> context)
    {
        var message = new Message
        {
            Content = context.Message.Content,
            Time = context.Message.Time,
            FileId = context.Message.Metadata.FileId
        };
        await _messageService.AddMessageAsync(message);
    }
}