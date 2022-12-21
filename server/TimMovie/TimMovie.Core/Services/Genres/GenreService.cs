using TimMovie.Core.Entities;
using TimMovie.Core.Specifications.InheritedSpecifications.GenreSpec;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Genres;

public class GenreService
{
    private readonly IRepository<Genre> _genreRepository;

    public GenreService(IRepository<Genre> genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public IEnumerable<string> GetGenreNames()
    {
        return _genreRepository.Query.Select(genre => genre.Name).ToList();
    }

    public IEnumerable<Genre> GetGenresByNamePart(string? namePart, int count = int.MaxValue) =>
        _genreRepository.Query.Where(new GenreByNamePartSpec(namePart)).Take(count);
}