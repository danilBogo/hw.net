using MassTransit;
using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Repositories;
using SupportChat.Infrastructure.Services;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using SupportChat.Domain.Configurations;

namespace SupportChat.WebHost.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSignalR();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<MessageService>();
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
                {
                    h.Username(configuration.GetRequiredSection("Rabbit:Username").Value);
                    h.Password(configuration.GetRequiredSection("Rabbit:Password").Value);
                });
                cfg.ReceiveEndpoint(e => { e.Bind(configuration.GetRequiredSection("Rabbit:ExchangeName").Value); });
                cfg.ConfigureEndpoints(context);
            });
        });

        var awsOptions = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(
                configuration["AWS:AccessKey"],
                configuration["AWS:AccessSecret"]),

            DefaultClientConfig =
            {
                ServiceURL = configuration["AWS:ServiceUrl"]
            }
        };

        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();
        return services;
    }
}