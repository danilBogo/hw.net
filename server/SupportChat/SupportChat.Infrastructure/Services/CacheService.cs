using SupportChat.Domain.Interfaces;

namespace SupportChat.Infrastructure.Services;

public class CacheService
{
    private readonly ICacheRepository _cacheRepository;

    public CacheService(ICacheRepository cacheRepository)
    {
        _cacheRepository = cacheRepository;
    }
}