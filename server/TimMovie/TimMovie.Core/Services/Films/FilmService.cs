using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TimMovie.Core.DTO.Actor;
using TimMovie.Core.DTO.Comments;
using TimMovie.Core.DTO.Country;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.DTO.Genre;
using TimMovie.Core.DTO.Producer;
using TimMovie.Core.Entities;
using TimMovie.Core.ExpressionQuery.Films;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Query;
using TimMovie.Core.Services.Subscribes;
using TimMovie.Core.Services.WatchedFilms;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Classes;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Films;

public class FilmService
{
    private readonly IRepository<Film> _filmRepository;
    private readonly IRepository<User> _userRepository;
    private readonly Lazy<WatchedFilmService> _watchedFilmService;
    private readonly ISubscribeService _subscribeService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public FilmService(
        IRepository<Film> filmRepository,
        IRepository<User> userRepository,
        IMapper mapper,
        Lazy<WatchedFilmService> watchedFilmService,
        UserManager<User> userManager, ISubscribeService subscribeService)
    {
        _filmRepository = filmRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _watchedFilmService = watchedFilmService;
        _userManager = userManager;
        _subscribeService = subscribeService;
    }

    private bool TryGetFirstOrDefaultFilm(Guid filmId, out Film? dbFilm)
    {
        dbFilm = _filmRepository.Query.FirstOrDefault(new EntityByIdSpec<Film>(filmId));
        return dbFilm is not null;
    }

    private bool TryGetFirstOrDefaultUser(Guid userId, out User? user)
    {
        user = _userRepository.Query.Include(u => u.FilmsWatchLater)
            .FirstOrDefault(new EntityByIdSpec<User>(userId));
        return user is not null;
    }

    public bool TryGetFilmAndUser(Guid filmId, Guid userId, out Film? dbFilm, out User? user) =>
        TryGetFirstOrDefaultFilm(filmId, out dbFilm)
        & TryGetFirstOrDefaultUser(userId, out user);

    public async Task<bool> TryUpdateUserRepository(User? user)
    {
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public bool TryGetUserGrade(Guid filmId, Guid userId, out int? grade)
    {
        grade = null;
        if (!TryGetFilmAndUser(filmId, userId, out var dbFilm, out var user)) return false;

        grade = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(filmId))
            .Select(f =>
                f.UserFilmWatcheds
                    .Where(watched => watched.WatchedUser == user)
                    .Select(watched => watched.Grade)
                    .FirstOrDefault())
            .FirstOrDefault();
        return true;
    }

    public async Task<Result<CommentsDto>> TryAddCommentToFilm(Guid? userId, Guid filmId, string content)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Fail<CommentsDto>("данного пользователя не существует");
        var dbFilm = GetDbFilmById(filmId);
        if (dbFilm is null)
            return Result.Fail<CommentsDto>("данного фильма не существует");
        if (content is null)
            return Result.Fail<CommentsDto>("комментарий не может быть пустым");
        switch (content.Length)
        {
            case < 2:
                return Result.Fail<CommentsDto>("комментарий слишком короткий");
            case > 1000:
                return Result.Fail<CommentsDto>("комментарий слишком длинный");
        }

        var comment = new Comment
        {
            Film = dbFilm,
            Author = user,
            Content = content,
            Date = DateTime.UtcNow
        };
        dbFilm.Comments.Add(comment);
        await _filmRepository.SaveChangesAsync();
        var resultComment = new CommentsDto
        {
            AuthorId = user.Id,
            AuthorPathToPhoto = user.PathToPhoto,
            AuthorDisplayName = user.DisplayName,
            Content = content,
            Date = DateTime.UtcNow
        };
        return Result.Ok(resultComment);
    }

    public async Task<bool> TryUpdateFilmGrade(Guid filmId, Guid userId, int grade)
    {
        var dbFilm = _filmRepository.Query.FirstOrDefault(new EntityByIdSpec<Film>(filmId));
        if (dbFilm is null)
            return false;

        var userQuery = _userRepository.Query.Where(new EntityByIdSpec<User>(userId));
        var userQueryExecutor = new QueryExecutor<User>(userQuery, _userRepository);
        var user = userQueryExecutor
            .IncludeInResult(user => user.WatchedFilms)
            .FirstOrDefault();
        if (user is null)
            return false;

        // ReSharper disable once ConstantNullCoalescingCondition
        user.WatchedFilms ??= new List<UserFilmWatched>();
        var watchedFilms = user.WatchedFilms.FirstOrDefault(watched => watched.Film == dbFilm);
        if (watchedFilms is null)
            user.WatchedFilms.Add(new UserFilmWatched
            {
                Date = DateTime.Now,
                Film = dbFilm,
                Grade = grade,
                WatchedUser = user
            });
        else
        {
            if (watchedFilms.Grade == grade)
                watchedFilms.Grade = null;
            else
                watchedFilms.Grade = grade;
        }

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public bool IsExistInSubscribe(Film film)
    {
        return _filmRepository.Query
            .FirstOrDefault(new EntityByIdSpec<Film>(film.Id)
                            && FilmStaticSpec.FilmIsIncludedAnySubscriptionSpec) is not null;
    }

    public double GetRating(Film film)
    {
        var rating = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(film.Id))
            .Select(FilmQueryExpression.Rating)
            .FirstOrDefault();
        return Math.Round(rating, 1);
    }

    public IEnumerable<Film> GetFilmsByNamePart(string? namePart, int count = int.MaxValue) =>
        _filmRepository.Query.Where(new FilmByNamePartSpec(namePart)).Take(count);

    public FilmForStatusDto? GetCurrentWatchingFilmByUser(Guid userId)
    {
        var query = _userRepository.Query
            .Where(new EntityByIdSpec<User>(userId));
        var executor = new QueryExecutor<User>(query, _userRepository);

        var film = executor
            .IncludeInResult(user => user.WatchingFilm)
            .FirstOrDefault();

        return MapToRequiredDto<User?, FilmForStatusDto>(film);
    }

    private TDto? MapToRequiredDto<T, TDto>(T entity)
        where TDto : class => entity is null
        ? null
        : _mapper.Map<TDto>(entity);
    
    public FilmDto? GetFilmById(Guid filmId, Guid? userId = null)
    {
        var dbFilm = GetDbFilmById(filmId);
        if (dbFilm == null) return null;
        var filmDto = new FilmDto
        {
            Id = dbFilm!.Id,
            Title = dbFilm.Title,
            Year = dbFilm.Year,
            Description = dbFilm.Description,
            Country = new CountryDto {Name = dbFilm.Country?.Name},
            Rating = GetRating(dbFilm),
            GradesNumber = _watchedFilmService.Value.GetAmountGradesForFilms(filmId),
            FilmLink = dbFilm.FilmLink,
            Comments = dbFilm.Comments.Select(comment => new CommentsDto
                {
                    AuthorDisplayName = comment.Author.DisplayName,
                    AuthorId = comment.Author.Id,
                    AuthorPathToPhoto = comment.Author.PathToPhoto,
                    Content = comment.Content,
                    Date = comment.Date
                }).OrderByDescending(c => c.Date)
                .ToList(),
            Producers = dbFilm.Producers.Select(producer => new ProducerDto
            {
                Id = producer.Id,
                Name = producer.Name,
                Surname = producer.Surname,
                Photo = producer.Photo
            }).ToList(),
            Actors = dbFilm.Actors.Select(actor => new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                Surname = actor.Surname,
                Photo = actor.Photo
            }).ToList(),
            Genres = dbFilm.Genres.Select(genre => new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name
            }).ToList(),
            IsAvailable = _subscribeService.IsFilmAvailableForUser(userId, dbFilm)
        };
        return filmDto;
    }

    public Film? GetDbFilmById(Guid filmId)
    {
        var query = _filmRepository.Query
            .Where(new EntityByIdSpec<Film>(filmId));
        var executor = new QueryExecutor<Film>(query, _filmRepository);

        return executor
            .IncludeInResult(film => film.Genres)
            .IncludeInResult(film => film.Country)
            .IncludeInResult(film => film.Actors)
            .IncludeInResult(film => film.Producers)
            .IncludeEnumerableInResult(film => film.Comments)
            .ThenIncludeInResult(comment => comment.Author)
            .FirstOrDefault();
    }
}