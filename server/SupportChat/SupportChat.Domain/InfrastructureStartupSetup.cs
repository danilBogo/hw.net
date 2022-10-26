using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportChat.Domain.Configurations;
using SupportChat.Domain.Database;
using SupportChat.Domain.Interfaces;

namespace SupportChat.Domain;

public static class InfrastructureStartupSetup
{
    public static void AddDbContext(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoDbConfiguration>(new MongoDbConfiguration
        {
            Database = configuration.GetRequiredSection("Mongo:DatabaseName").Value,
            ConnectionString = configuration.GetRequiredSection("Mongo:ConnectionString").Value 
        });
        
        return services;
    }
}