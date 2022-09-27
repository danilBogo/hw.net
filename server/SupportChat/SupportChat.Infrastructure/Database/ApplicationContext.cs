using Microsoft.EntityFrameworkCore;
using SupportChat.Core.Entities;

namespace SupportChat.Infrastructure.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<User> Users => Set<User>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
}