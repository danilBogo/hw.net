using AutoMapper;
using TimMovie.Core.Classes;
using TimMovie.Core.DTO.WatchedFilms;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec.UserFilmWatchedSpec;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.WatchedFilms;

public class WatchedFilmService
{
    private readonly IRepository<UserFilmWatched> userFilmWatchedRepository;
    private readonly IMapper mapper;
    private readonly FilmService filmService;

    public WatchedFilmService(IRepository<UserFilmWatched> userFilmWatchedRepository,
        IMapper mapper,
        FilmService filmService)
    {
        this.userFilmWatchedRepository = userFilmWatchedRepository;
        this.mapper = mapper;
        this.filmService = filmService;
    }

    public PaginatedList<WatchedFilmDto> GetPaginatedUserWatchedFilmsByUserId(Guid userId,int pageIndex, int pageSize)
    {
        var query = userFilmWatchedRepository.Query
            .Where(new WatchedFilmByUserIdSpec(userId))
            .OrderByDescending(watched => watched.Date);
        
        var paginatedQuery = query            
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        var queryExec = new QueryExecutor<UserFilmWatched>(paginatedQuery, userFilmWatchedRepository);
        var films = queryExec
            .IncludeInResult(x => x.Film)
            .GetEntities();
        
        var watchedDto = films.Select(x =>
        {
            var watchedDto = mapper.Map<WatchedFilmDto>(x);
            watchedDto.Rating = filmService.GetRating(x.Film);
            return watchedDto;
        });

        return new PaginatedList<WatchedFilmDto>(watchedDto, query.Count(), pageIndex, pageSize);
    }

    public int GetAmountGradesForFilms(Guid filmId)
    {
        return userFilmWatchedRepository.Query
            .Where(new WatchedFilmByFilmIdSpec(filmId) && WatchedFilmStaticSpec.HaveGrade)
            .Count();
    }
}