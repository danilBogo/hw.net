using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;

/// <summary>
/// Фильтр на минимальный рейтинг.
/// </summary>
public class FilmWithMinimumRatingSpec: Specification<Film>
{
    /// <summary>
    /// Создает фильтр, который отбирает фильмы, рейтинг которых больше <paramref name="rating"/>.
    /// </summary>
    /// <param name="rating">Минимальный рейтинг.</param>
    public FilmWithMinimumRatingSpec(double rating)
    {
        Conditional = film =>
            film.UserFilmWatcheds.Select(watched => watched.Grade ?? 0).Average() >= rating;
    }
}