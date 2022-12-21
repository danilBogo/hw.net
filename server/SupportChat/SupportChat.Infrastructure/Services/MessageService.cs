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

    public async Task<IEnumerable<Message>> GetMessageHistoryByUserNameAndInterlocutorNameAsync(string userName,
        string? interlocutor)
    {
        return await _messageRepository.GetByUserNameAsync(userName, interlocutor);
    }

    public async Task AddMessageAsync(Message message)
    {
        await _messageRepository.AddAsync(message);
    }
}