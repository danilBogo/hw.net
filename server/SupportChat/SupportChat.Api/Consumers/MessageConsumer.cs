using MassTransit;
using SupportChat.Core.Entities;
using SupportChat.Core.Services;

namespace SupportChat.Api.Consumers;

public class MessageConsumer : IConsumer<Message>
{
    private readonly MessageService _messageService;

    public MessageConsumer(MessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task Consume(ConsumeContext<Message> context)
    {
        var message = new Message
        {
            Content = context.Message.Content,
            Time = context.Message.Time
        };
        await _messageService.AddMessageAsync(message);
    }
}