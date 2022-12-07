using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;
using SupportChat.Domain.Configuration;
using SupportChat.Domain.Database;
using SupportChat.Domain.Interfaces;

namespace SupportChat.Domain;

public static class InfrastructureStartupSetup
{
    public static void AddDbContext(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<ApplicationContext>(options => { options.UseNpgsql(connectionString); });

    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMongoDatabase>(sp =>
            new MongoClient(configuration.GetRequiredSection("Mongo:ConnectionString").Value)
                .GetDatabase(configuration
                    .GetRequiredSection("Mongo:DatabaseName").Value));
        return services;
    }

    public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        Environment.SetEnvironmentVariable("AWS_ENABLE_ENDPOINT_DISCOVERY", "false");
        Environment.SetEnvironmentVariable("endpoint_discovery_enabled", "false");
        var credentials = new BasicAWSCredentials(configuration["AWS:AccessKey"], configuration["AWS:AccessSecret"]);
        var awsOptions = new AWSOptions
        {
            Credentials = credentials,

            DefaultClientConfig =
            {
                ServiceURL = configuration["AWS:ServiceUrl"],
            },
            Region = RegionEndpoint.EUWest1
        };

        services.AddDefaultAWSOptions(awsOptions);
        services.AddSingleton<IAmazonConfig>(new AmazonConfig
        {
            AccessKey = configuration["AWS:AccessKey"],
            AccessSecret = configuration["AWS:AccessSecret"],
            ServiceUrl = configuration["AWS:ServiceUrl"]
        });
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new ConfigurationOptions
        {
            EndPoints = new EndPointCollection
                { { configuration["Redis:Host"], int.Parse(configuration["Redis:Port"]) } },
        };
        var redis = ConnectionMultiplexer.Connect(options);
        services.AddSingleton<IConnectionMultiplexer>(redis);
        return services;
    }
}