using SupportChat.Core.Entities;
using SupportChat.Core.Interfaces;

namespace SupportChat.Core.Services;

public class MessageService
{
    private readonly IMessageRepository messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        this.messageRepository = messageRepository;
    }

    public async Task<IEnumerable<Message>> GetMessageHistoryAsync()
    {
        return await messageRepository.GetAllAsync();
    }

    public async Task AddMessageAsync(Message message)
    {
        await messageRepository.AddAsync(message);
        await messageRepository.SaveChangesAsync();
    }
}