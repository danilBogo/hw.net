namespace SupportChat.Domain.Interfaces;

public interface ICacheRepository
{
    public Task Add(string key, string value);

    public Task<string> GetByKey(string key);

    public Task<long> Incr(string incrKey);
}