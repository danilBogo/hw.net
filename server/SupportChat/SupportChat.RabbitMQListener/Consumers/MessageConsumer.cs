using MassTransit;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Models;
using SupportChat.Infrastructure.Services;

namespace SupportChat.RabbitMQListener.Consumers;

public class MessageConsumer : IConsumer<MessageDto>
{
    private readonly MessageService _messageService;
    private readonly FileService _fileService;

    public MessageConsumer(MessageService messageService, FileService fileService)
    {
        _messageService = messageService;
        _fileService = fileService;
    }

    public async Task Consume(ConsumeContext<MessageDto> context)
    {
        var message = new Message
        {
            Content = context.Message.Content,
            Time = context.Message.Time
        };
        await _messageService.AddMessageAsync(message);
        var file = _fileService.GetFile(context.Message.JsonMetadata,
            Path.GetExtension(context.Message.FormFile.FileName));
        // _fileService.CreateFileMetaData(file);
    }
}