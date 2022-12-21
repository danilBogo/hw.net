using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;

/// <summary>
/// Фильтр по названию жанра фильма.
/// </summary>
public class FilmByGenreNamesSpec : Specification<Film>
{
    /// <summary>
    /// Фильтр, который отбирает все фильмы, которые принадлежат хотя бы одному жанру из списка.
    /// </summary>
    /// <param name="genreNames">Список имен жанров.</param>
    /// <exception cref="ArgumentNullException"><paramref name="genreNames"/> равен null.</exception>
    public FilmByGenreNamesSpec(params string[] genreNames)
    {
        ArgumentValidator.ThrowExceptionIfNull(genreNames, nameof(genreNames));

        if (!genreNames.Any())
        {
            Conditional = film => true;
            return;
        }

        var conditional = new Specification<Film>(film =>
            film.Genres.Select(genre => genre.Name).Contains(genreNames.FirstOrDefault()));
        foreach (var name in genreNames.Skip(1))
        {
            conditional = conditional && new Specification<Film>(
                film => film.Genres.Select(genre => genre.Name).Contains(name));
        }

        Conditional = conditional.ToExpression();
    }
}