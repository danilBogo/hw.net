using Microsoft.EntityFrameworkCore;
using SupportChat.Domain.Database;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models;

namespace SupportChat.Domain.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationContext _context;


    public MessageRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Message> AddAsync(Message message)
    {
        var entityEntry = await _context.Messages.AddAsync(message);
        await SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<IEnumerable<Message>> GetByUserNameAsync(string userName, string? interlocutor) =>
        await _context.Messages
            .Where(m => m.UserName == userName && m.InterlocutorName == interlocutor || 
                        m.UserName == interlocutor && m.InterlocutorName == userName)
            .OrderBy(m => m.Time)
            .ToListAsync();

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}