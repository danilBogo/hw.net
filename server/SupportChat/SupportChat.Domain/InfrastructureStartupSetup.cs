using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon;
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
        services.AddDbContext<ApplicationContext>(options => { options.UseNpgsql(connectionString); });

    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoDbConfiguration>(new MongoDbConfiguration
        {
            Database = configuration.GetRequiredSection("Mongo:DatabaseName").Value,
            ConnectionString = configuration.GetRequiredSection("Mongo:ConnectionString").Value
        });

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
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ForcePathStyle = true,
            ServiceURL = configuration["AWS:ServiceUrl"],
        };
        var client = new AmazonS3Client(credentials, config);
        services.AddSingleton<IAmazonS3>(client);
        return services;
    }
}