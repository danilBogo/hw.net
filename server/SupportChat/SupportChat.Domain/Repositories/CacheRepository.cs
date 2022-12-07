using StackExchange.Redis;
using SupportChat.Domain.Interfaces;

namespace SupportChat.Domain.Repositories;

public class CacheRepository : ICacheRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CacheRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task Add(string key, string value)
    {
        var database = _connectionMultiplexer.GetDatabase();
        await database.HashSetAsync(key, key, value);
    }

    public async Task<long> Incr(string incrKey)
    {
        var database = _connectionMultiplexer.GetDatabase();
        return await database.HashIncrementAsync(incrKey, incrKey);
    }

    public async Task<string> GetByKey(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();
        var result = await database.HashGetAsync(key, key);
        if (result.IsNull)
            throw new Exception("Value in redis is not exists");
        return result!;
    }
}