using Microsoft.EntityFrameworkCore;
using SupportChat.Domain.Models;

namespace SupportChat.Domain.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Message> Messages => Set<Message>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}