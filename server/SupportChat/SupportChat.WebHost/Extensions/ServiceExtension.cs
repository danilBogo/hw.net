using MassTransit;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Repositories;
using SupportChat.Infrastructure.Services;
using SupportChat.WebHost.Consumers;

namespace SupportChat.WebHost.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSignalR();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<MessageService>();
        services.AddMassTransit(config =>
        {
            config.AddConsumer<MessageConsumer>();
            config.UsingInMemory((context, configurator) =>
            {
                configurator.Host();
                configurator.ReceiveEndpoint(new TemporaryEndpointDefinition(), x =>
                {
                    x.ConfigureConsumer<MessageConsumer>(context);
                });
                configurator.ConfigureEndpoints(context);
            });
        });
        services.AddScoped<MessageConsumer>();
        return services;
    }
}