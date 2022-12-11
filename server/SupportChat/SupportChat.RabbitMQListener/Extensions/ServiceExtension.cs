using MassTransit;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Repositories;
using SupportChat.Infrastructure.Services;
using SupportChat.RabbitMQListener.Consumers;

namespace SupportChat.RabbitMQListener.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddScoped<MessageService>();
        services.AddScoped<MetadataService>();
        services.AddScoped<ICacheRepository, CacheRepository>();
        services.AddScoped<CacheService>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<FileService>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IMetadataRepository, MetadataRepository>();
        services.AddMassTransit(config =>
        {
            config.AddConsumer<MessageConsumer>();
            config.AddConsumer<FileUploadConsumer>();
            config.UsingRabbitMq((context, cfg) =>
            {
                //cfg.Host(configuration.GetRequiredSection("Rabbit:DockerImage").Value, "/", h =>
                cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
                {
                    h.Username(configuration.GetRequiredSection("Rabbit:Username").Value);
                    h.Password(configuration.GetRequiredSection("Rabbit:Password").Value);
                });
                cfg.ReceiveEndpoint(e =>
                {
                    e.Bind(configuration.GetRequiredSection("Rabbit:ExchangeName").Value);
                    e.ConfigureConsumer<MessageConsumer>(context);
                    e.ConfigureConsumer<FileUploadConsumer>(context);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}