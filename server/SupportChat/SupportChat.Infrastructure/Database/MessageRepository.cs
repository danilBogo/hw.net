using Microsoft.EntityFrameworkCore;
using SupportChat.Core.Entities;
using SupportChat.Core.Interfaces;

namespace SupportChat.Infrastructure.Database;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationContext context;


    public MessageRepository(ApplicationContext context)
    {
        this.context = context;
    }

    public async Task<Message> AddAsync(Message message)
    {
        var entityEntry = await context.Messages.AddAsync(message);
        await SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await context.Messages.OrderBy(m => m.Time).ToListAsync();
    }

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}