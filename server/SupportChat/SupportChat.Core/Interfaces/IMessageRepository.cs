using SupportChat.Core.Entities;

namespace SupportChat.Core.Interfaces;

public interface IMessageRepository
{
    Task<Message> AddAsync(Message item);
    Task<IEnumerable<Message>> GetAllAsync();
    Task SaveChangesAsync();
}