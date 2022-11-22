using EasyCaching.Core;
using SupportChat.Domain.Interfaces;

namespace SupportChat.Domain.Repositories;

public class CacheRepository : ICacheRepository
{
    private readonly IRedisCachingProvider _provider;

    public CacheRepository(IEasyCachingProviderFactory factory)
    {
        _provider = factory.GetRedisProvider("default");
    }

    public async Task<bool> Add(string key, string value)
    {
        var result = await _provider.StringSetAsync(key, value);
        // if (result)
        // {
        //     _provider.IncrByAsync();
        // }
        return await _provider.StringSetAsync(key, value);
    }

    public async Task<string> GetByKey(string key) => await _provider.StringGetAsync(key);
}