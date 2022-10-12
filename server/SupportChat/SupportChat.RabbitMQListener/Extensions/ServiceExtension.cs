using MassTransit;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Repositories;
using SupportChat.RabbitMQListener.Consumers;

namespace SupportChat.RabbitMQListener.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddMassTransit(config =>
        {
            config.AddConsumer<MessageConsumer>();
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetRequiredSection("Rabbit:DockerImage").Value, "/", h =>
                {
                    h.Username(configuration.GetRequiredSection("Rabbit:Username").Value);
                    h.Password(configuration.GetRequiredSection("Rabbit:Password").Value);
                });
                cfg.ReceiveEndpoint(e =>
                {
                    e.Bind(configuration.GetRequiredSection("Rabbit:ExchangeName").Value);
                    e.ConfigureConsumer<MessageConsumer>(context);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}