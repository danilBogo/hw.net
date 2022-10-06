using SupportChat.Domain.Models;

namespace SupportChat.Domain.Interfaces;

public interface IMessageRepository
{
    Task<Message> AddAsync(Message item);
    Task<IEnumerable<Message>> GetAllAsync();
    Task SaveChangesAsync();
}