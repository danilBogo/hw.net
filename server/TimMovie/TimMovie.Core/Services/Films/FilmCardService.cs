using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.DTO;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Query.Films;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec.UserFilmWatchedSpec;
using TimMovie.SharedKernel.Classes;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Core.Services.Films;

public class FilmCardService
{
    private readonly IRepository<Film> _filmRepository;
    private readonly IRepository<UserFilmWatched> _userFilmWatchedRepository;
    private readonly IMapper _mapper;
    private readonly FilmService _filmService;
    private readonly WatchLaterService _watchLaterService;


    public FilmCardService(
        IRepository<Film> filmRepository,
        IMapper mapper,
        FilmService filmService,
        IRepository<UserFilmWatched> userFilmWatchedRepository, WatchLaterService watchLaterService)
    {
        _filmRepository = filmRepository;
        _mapper = mapper;
        _filmService = filmService;
        _userFilmWatchedRepository = userFilmWatchedRepository;
        _watchLaterService = watchLaterService;
    }

    public IEnumerable<FilmCardDto> GetFilmCardsByFilters(
        GeneralPaginationDto<SelectedFilmFiltersDto> filtersWithPagination, Guid userId = default)
    {
        ArgumentValidator.ThrowExceptionIfNull(filtersWithPagination, nameof(filtersWithPagination));
        ArgumentValidator.ThrowExceptionIfNull(
            filtersWithPagination.DataDto,
            nameof(filtersWithPagination.DataDto));

        var filters = filtersWithPagination.DataDto;

        var queryExecutor = GetQueryExecutorWithFiltersAndSort(filters);

        var films = GetFilmsWithCountryAndGenres(filtersWithPagination, queryExecutor);

        var filmCard = GetFilmCardsByFilms(films, userId);

        return filmCard;
    }

    private List<FilmCardDto> GetFilmCardsByFilms(IEnumerable<Film> films, Guid userId = default)
    {
        return films
            .Select(film =>
            {
                var filmCard = _mapper.Map<FilmCardDto>(film);
                filmCard.Rating = _filmService.GetRating(film);
                if (userId != default)
                    AddGradeAndWatchLater(filmCard, userId);
                return filmCard;
            })
            .ToList();
    }

    private static IEnumerable<Film> GetFilmsWithCountryAndGenres(
        GeneralPaginationDto<SelectedFilmFiltersDto> filtersWithPagination,
        QueryExecutor<Film> queryExecutor)
    {
        return queryExecutor
            .IncludeInResult(film => film.Genres)
            .IncludeInResult(film => film.Country)
            .GetEntitiesWithPagination(filtersWithPagination.AmountSkip, filtersWithPagination.AmountTake);
    }

    private QueryExecutor<Film> GetQueryExecutorWithFiltersAndSort(SelectedFilmFiltersDto filters)
    {
        var filterBuilder = new FilmFiltersBuilder(_filmRepository);
        filterBuilder
            .AddFilterByGenre(filters.GenresName)
            .AddFilterByYear(filters.AnnualPeriod.FirstYear, filters.AnnualPeriod.LastYear)
            .AddFilterByCountry(filters.CountriesName)
            .AddFilterOnMinimumRating(filters.Rating ?? 0);

        var sortBuilder = new SortFilmBuilder(filterBuilder);
        sortBuilder.AddSortByTypeSort(filters.SortingType, filters.IsDescending);

        return sortBuilder.Build();
    }


    public List<FilmCardDto> GetFilmCardsByGenre(string genreName, int amount, Guid? filmToRemoveId,
        Guid userId = default)
    {
        var isNeedToRemove = filmToRemoveId is not null;
        if (isNeedToRemove) amount += 1;

        var filterBuilder = new FilmFiltersBuilder(_filmRepository);
        filterBuilder.AddFilterByGenre(new[] {genreName});

        var sortBuilder = new SortFilmBuilder(filterBuilder);
        sortBuilder.AddSortByPopularity(true);

        var queryExecutor = sortBuilder.Build();

        var films = queryExecutor
            .IncludeInResult(film => film.Genres)
            .IncludeInResult(film => film.Country)
            .GetEntitiesWithPagination(0, amount);
        
        if (isNeedToRemove)
            films = films.Where(film => film.Id != filmToRemoveId).ToList();

        return GetFilmCardsByFilms(films, userId);
    }

    private void AddGradeAndWatchLater(FilmCardDto filmCardDto, Guid userId)
    {
        _filmService.TryGetUserGrade(filmCardDto.Id, userId, out var tmpGrade);
        filmCardDto.IsGradeSet = tmpGrade is not null;
        filmCardDto.IsAddedToWatchLater = _watchLaterService.IsWatchLaterFilm(filmCardDto.Id, userId);
    }

    public IEnumerable<FilmCardDto> GetLatestFilmsViewedByUser(Guid guestId, int amount, Guid? currentUser)
    {
        var query = _userFilmWatchedRepository.Query
            .Where(new WatchedFilmByUserIdSpec(guestId))
            .OrderByDescending(watched => watched.Date);
        var queryExecutor = new QueryExecutor<UserFilmWatched>(query, _userFilmWatchedRepository);

        var films = queryExecutor
            .IncludeInResult(watched => watched.Film.Country)
            .IncludeInResult(watched => watched.Film.Genres)
            .GetEntitiesWithPagination(0, amount)
            .Select(watched => watched.Film);

        return GetFilmCardsByFilms(films, currentUser ?? default);
    }
    
    public Result<IEnumerable<FilmCardDto>> GetFilmRecommendationsByUserId(Guid userId, int amount)
    {
        if (amount < 0)
            return Result.Fail<IEnumerable<FilmCardDto>>("значение не может быть отрицательным");
        var query = _userFilmWatchedRepository.Query
            .Where(new WatchedFilmByUserIdSpec(userId))
            .OrderByDescending(watched => watched.Grade ?? 0);
        var queryExec = new QueryExecutor<UserFilmWatched>(query, _userFilmWatchedRepository);

        var bestGradedFilm =
            queryExec
                .IncludeInResult(watched => watched.Film.Producers)
                .IncludeInResult(watched => watched.Film.Actors)
                .GetEntitiesWithPagination(0, 1)
                .FirstOrDefault();
        if (bestGradedFilm == null)
        {
            return Result.Fail<IEnumerable<FilmCardDto>>("у данного пользователя нет оцененных фильмов");
        }

        var producer = bestGradedFilm.Film.Producers.FirstOrDefault();

        var actor = bestGradedFilm.Film.Actors.FirstOrDefault();

        if (producer == null || actor == null)
        {
            return Result.Fail<IEnumerable<FilmCardDto>>("у фильма с лучшим рейтингом нет продюссера/актеров");
        }

        var filmQuery = _filmRepository.Query
            .Where((new FilmWithProducerSpec(producer.Id) || new FilmWithActorSpec(actor.Id)) &&
                   new ExceptEntityByIdSpec<Film>(bestGradedFilm.Id));

        var filterBuilder = new FilmFiltersBuilder(_filmRepository, filmQuery);
        var sortBuilder = new SortFilmBuilder(filterBuilder);
        sortBuilder.AddSortByRating(true);

        var queryExecutor = sortBuilder.Build();

        var films = queryExecutor
            .IncludeInResult(x => x.Country)
            .IncludeInResult(x => x.Genres)
            .GetEntitiesWithPagination(0, amount);

        var result = GetFilmCardsByFilms(films, userId);

        if (result.Any())
        {
            return Result.Ok<IEnumerable<FilmCardDto>>(result);
        }

        return Result.Fail<IEnumerable<FilmCardDto>>("не найдены фильмы для рекомендации");
    }
}