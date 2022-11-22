namespace SupportChat.Domain.Interfaces;

public interface ICacheRepository
{
    public Task<bool> Add(string key, string value);

    public Task<string> GetByKey(string key);
}