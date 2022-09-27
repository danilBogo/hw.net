using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportChat.Infrastructure.Database;

namespace SupportChat.Infrastructure;

public static class InfrastructureStartupSetup
{
    public static void AddDbContext(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
}