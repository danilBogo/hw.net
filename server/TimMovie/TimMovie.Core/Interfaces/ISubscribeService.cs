using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.Entities;

namespace TimMovie.Core.Interfaces;

public interface ISubscribeService
{
    public IEnumerable<SubscribeDto> GetSubscribesByNamePart(string? namePart, int take = int.MaxValue, int skip = 0);

    public IEnumerable<UserSubscribeDto> GetAllActiveUserSubscribes(Guid? userId);

    public bool IsFilmAvailableForUser(Guid? userId, Film? film);

    public Task AddUserToSubscribeAsync(User user, Subscribe subscribe);

    public Task AddUserToSubscribeAsync(User user, Subscribe subscribe, DateTime startDate, DateTime endDate);

    public Subscribe? GetSubscribeById(Guid subscribeId);
}