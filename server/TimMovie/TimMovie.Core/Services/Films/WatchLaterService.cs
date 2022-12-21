using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.Core.Query.Films;
using TimMovie.Core.Services.WatchedFilms;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Films;

public class WatchLaterService
{
    private readonly FilmService _filmService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public WatchLaterService(
        IMapper mapper,
        UserManager<User> userManager, FilmService filmService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _filmService = filmService;
    }
    
    public bool IsWatchLaterFilm(Guid filmId, Guid userId)
    {
        if (!_filmService.TryGetFilmAndUser(filmId, userId, out var dbFilm, out var user))
            return false;
        return user!.FilmsWatchLater.Contains(dbFilm!);
    }
    
    public async Task<bool> TryAddFilmToWatchLater(Guid filmId, Guid userId)
    {
        if (!_filmService.TryGetFilmAndUser(filmId, userId, out var dbFilm, out var user)) return false;

        user!.FilmsWatchLater.Add(dbFilm!);
        return await _filmService.TryUpdateUserRepository(user);
    }

    public async Task<bool> TryRemoveFilmFromWatchLater(Guid filmId, Guid userId)
    {
        if (!_filmService.TryGetFilmAndUser(filmId, userId, out var dbFilm, out var user)) return false;

        user!.FilmsWatchLater.Remove(dbFilm!);
        return await _filmService.TryUpdateUserRepository(user);
    }
    
    
    public FilmDto GetFilmForBigCardById(Guid filmId)
    {
        var dbFilm = _filmService.GetDbFilmById(filmId);
        var filmDto = _mapper.Map<FilmDto>(dbFilm);
        filmDto!.Rating = _filmService.GetRating(dbFilm!);
        return filmDto;
    }

    public List<BigFilmCardDto> GetWatchLaterFilms(Guid userId, int take, int skip)
    {
        if (take < 0 || skip < 0)
            return new List<BigFilmCardDto>();
        var filmsWatchLater = _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => u.FilmsWatchLater.Skip(skip).Take(take))
            .FirstOrDefault();

        if (filmsWatchLater is null) return new List<BigFilmCardDto>();

        var filmDtoList = filmsWatchLater.Select(f => GetFilmForBigCardById(f.Id));
        return filmDtoList
            .Select(filmDto => _mapper.Map<BigFilmCardDto>(filmDto))
            .ToList();
    }
}