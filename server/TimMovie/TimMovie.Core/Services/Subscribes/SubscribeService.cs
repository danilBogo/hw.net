using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Query;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.SubscribeSpec;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Subscribes;

public class SubscribeService : ISubscribeService
{
    private readonly IRepository<UserSubscribe> _userSubscribeRepository;
    private readonly IRepository<Subscribe> _subscribesRepository;
    private readonly IRepository<Genre> _genresRepository;
    private readonly IMapper _mapper;
    private const int SubscribeMonthsDuration = 1;

    public SubscribeService(IRepository<UserSubscribe> userSubscribeRepository,
        IRepository<Subscribe> subscribesRepository, IMapper mapper, IRepository<Genre> genresRepository)
    {
        _userSubscribeRepository = userSubscribeRepository;
        _subscribesRepository = subscribesRepository;
        _mapper = mapper;
        _genresRepository = genresRepository;
    }

    /// <summary>
    /// при namePart == null будут возвращены все подписки
    /// </summary>
    /// <param name="namePart"></param>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public IEnumerable<SubscribeDto> GetSubscribesByNamePart(string? namePart, int take = int.MaxValue, int skip = 0)
    {
        if (skip < 0 || take < 0)
            return new List<SubscribeDto>();
        var query = _subscribesRepository.Query
            .Where(new UserSubscribeByNamePartSpec(namePart) && SubscribeSpec.ActiveSubscribe)
            .Skip(skip)
            .Take(take);
        var subscribes = new QueryExecutor<Subscribe>(query, _subscribesRepository)
            .IncludeInResult(subscribe => subscribe.Films)
            .IncludeInResult(subscribe => subscribe.Genres)
            .GetEntities();
        return MapToSubscribeDto(subscribes);
    }

    public IEnumerable<UserSubscribeDto> GetAllActiveUserSubscribes(Guid? userId)
    {
        var query = _userSubscribeRepository.Query
            .Where(new UserSubscribeByUserIdSpec(userId) && SubscribeSpec.ActiveUserSubscribe);
        var subscribes = new QueryExecutor<UserSubscribe>(query, _userSubscribeRepository)
            .IncludeInResult(subscribe => subscribe.Subscribe)
            .GetEntities();
        return MapToUserSubscribeDto(subscribes);
    }

    public bool IsFilmAvailableForUser(Guid? userId, Film? film)
    {
        if (film.IsFree) return true;
        if (userId == null) return false;
        var userSubscribes = GetAllActiveUserSubscribes(userId);
        var subscribes = userSubscribes.Select(us => GetSubscribeById(us.SubscribeId));
        var isFilmInSubscribeFilms  = subscribes.Any(s => s.Films.FirstOrDefault(f => f.Id == film.Id) != null);
        var isFilmInSubscribeGenres = subscribes.Any(s => s.Genres.FirstOrDefault(g => g.Films.Contains(film)) != null);
        return isFilmInSubscribeFilms || isFilmInSubscribeGenres;
    }

    private IEnumerable<SubscribeDto> MapToSubscribeDto(IEnumerable<Subscribe> userSubscribes) =>
        userSubscribes.Select(s => _mapper.Map<SubscribeDto>(s));

    private IEnumerable<UserSubscribeDto> MapToUserSubscribeDto(IEnumerable<UserSubscribe> userSubscribes)
    {
        return userSubscribes
            .Select(subscribe => _mapper.Map<UserSubscribeDto>(subscribe));
    }

    public async Task AddUserToSubscribeAsync(User user, Subscribe subscribe)
    {
        await AddUserToSubscribeAsync(user, subscribe, DateTime.Now, DateTime.Now.AddMonths(SubscribeMonthsDuration));
    }


    public async Task AddUserToSubscribeAsync(User user, Subscribe subscribe, DateTime startDate, DateTime endDate)
    {
        var query = _userSubscribeRepository.Query
            .Where(new UserSubscribeByUserIdSpec(user.Id) && new UserSubscribesBySubscribeIdSpec(subscribe.Id) &&
                   SubscribeSpec.ActiveUserSubscribe);

        if (query.Any())
        {
            await ExtendUserSubscribeAsync(query.First());
            return;
        }

        var userSubscribe = new UserSubscribe
        {
            EndDate = endDate,
            StartDay = startDate,
            SubscribedUser = user,
            Subscribe = subscribe
        };
        await _userSubscribeRepository.AddAsync(userSubscribe);
        await _userSubscribeRepository.SaveChangesAsync();
    }

    Subscribe? ISubscribeService.GetSubscribeById(Guid subscribeId)
    {
        return GetSubscribeById(subscribeId);
    }

    private Subscribe? GetSubscribeById(Guid subscribeId) =>
        _subscribesRepository.Query.Include(s=>s.Films).Include(s=>s.Genres).ThenInclude(g => g.Films)
            .FirstOrDefault(new EntityByIdSpec<Subscribe>(subscribeId));

    private async Task ExtendUserSubscribeAsync(UserSubscribe userSubscribe)
    {
        userSubscribe.EndDate = userSubscribe.EndDate.AddMonths(SubscribeMonthsDuration);
        await _userSubscribeRepository.SaveChangesAsync();
    }
}