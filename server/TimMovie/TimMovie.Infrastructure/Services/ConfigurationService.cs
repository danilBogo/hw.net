using Microsoft.Extensions.Configuration;
using TimMovie.Core.Interfaces;

namespace TimMovie.Infrastructure.Services;

public class ConfigurationService: IConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetFileServiceUri()
    {
        return _configuration.GetRequiredSection("FileService:Uri").Value;
    }
}