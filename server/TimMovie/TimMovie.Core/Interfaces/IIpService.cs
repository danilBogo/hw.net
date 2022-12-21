using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IIpService
{
    public Task<Result<string>> GetCountryByIpAsync(string? ipAddress);
}