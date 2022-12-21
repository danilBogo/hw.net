using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using TimMovie.Core.Interfaces;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Infrastructure.Services;

public class IpService : IIpService
{
    public async Task<Result<string>> GetCountryByIpAsync(string? ipAddress) 
    {
        try
        {
            var request = WebRequest.Create("https://www.reg.ru/misc/geoip_lookup?ip_address_or_host=" + ipAddress);
            var response = await request.GetResponseAsync();
            await using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            var responseJson = await reader.ReadToEndAsync();
            var country = JsonDocument.Parse(responseJson).RootElement.GetProperty("country").GetProperty("ru").ToString();
            return Result.Ok(country);
        }
        catch (Exception e)
        {
            return Result.Fail<string>(e.Message);
        }
    }

}