using SupportChat.Domain.Enums;
using SupportChat.Domain.Interfaces;

namespace SupportChat.Infrastructure.Services;

public class CacheService
{
    private readonly ICacheRepository _cacheRepository;

    public CacheService(ICacheRepository cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }

    public async Task Add(RedisHeaderRecord redisHeaderRecord, string requestId, string value)
    {
        if (redisHeaderRecord == RedisHeaderRecord.Counter)
            throw new Exception($"Header record can not be {RedisHeaderRecord.Counter.ToString()}");
        var key = GetKeyByHeaderAndRequestId(redisHeaderRecord, requestId);
        await _cacheRepository.Add(key, value);
    }

    public async Task<long> Incr(string requestId)
    {
        var incrKey = GetKeyByHeaderAndRequestId(RedisHeaderRecord.Counter, requestId);
        return await _cacheRepository.Incr(incrKey);
    }

    public async Task<string> GetByKey(string key) => await _cacheRepository.GetByKey(key);

    public async Task<string> GetByHeaderKeyAndRequestId(RedisHeaderRecord redisHeaderRecord, string requestId) =>
        await GetByKey(GetKeyByHeaderAndRequestId(redisHeaderRecord, requestId));

    private string GetKeyByHeaderAndRequestId(RedisHeaderRecord redisHeaderRecord, string requestId) =>
        $"{redisHeaderRecord.ToString()}-{requestId}";
}