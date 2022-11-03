using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models;

namespace SupportChat.Infrastructure.Services;

public class MessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<IEnumerable<Message>> GetMessageHistoryAsync()
    {
        return await _messageRepository.GetAllAsync();
    }

    public async Task AddMessageAsync(Message message)
    {
        await _messageRepository.AddAsync(message);
    }
}