using TimMovie.Core.DTO;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.Actors;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Genres;
using TimMovie.Core.Services.Producers;

namespace TimMovie.Infrastructure.Services;

public class SearchEntityService : ISearchEntityService
{
    private readonly FilmService _filmService;
    private readonly ActorService _actorService;
    private readonly ProducerService _producerService;
    private readonly GenreService _genreService;

    public SearchEntityService(FilmService filmService, ActorService actorService, ProducerService producerService,
        GenreService genreService)
    {
        _filmService = filmService;
        _actorService = actorService;
        _producerService = producerService;
        _genreService = genreService;
    }

    /// <summary>
    /// Всего 10 записей: 4 фильма, 2 жанра, 2 актера, 2 режиссера
    /// </summary>
    public SearchEntityResultDto GetSearchEntityResultByNamePart(string? namePart)
    {
        if (namePart is null)
            return new SearchEntityResultDto
            {
                Films = new List<Film>(),
                Genres = new List<Genre>(),
                Actors = new List<Actor>(),
                Producers = new List<Producer>()
            };
        var result = new SearchEntityResultDto
        {
            Films = _filmService.GetFilmsByNamePart(namePart, 4),
            Genres = _genreService.GetGenresByNamePart(namePart, 2),
            Actors = _actorService.GetActorsByNamePart(namePart, 2),
            Producers = _producerService.GetProducersByNamePart(namePart, 2)
        };
        return result;
    }
}