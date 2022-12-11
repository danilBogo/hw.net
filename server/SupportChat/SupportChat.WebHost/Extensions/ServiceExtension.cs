using MassTransit;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Repositories;
using SupportChat.Infrastructure.Services;

namespace SupportChat.WebHost.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSignalR();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IMetadataRepository, MetadataRepository>();
        services.AddScoped<MessageService>();
        services.AddScoped<MetadataService>();
        services.AddScoped<CacheService>();
        services.AddScoped<ICacheRepository, CacheRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<FileService>();
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, cfg) =>
            {
                //cfg.Host(configuration.GetRequiredSection("Rabbit:DockerImage").Value, "/", h =>
                cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
                {
                    h.Username(configuration.GetRequiredSection("Rabbit:Username").Value);
                    h.Password(configuration.GetRequiredSection("Rabbit:Password").Value);
                });
                cfg.ReceiveEndpoint(e => { e.Bind(configuration.GetRequiredSection("Rabbit:ExchangeName").Value); });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}