using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;

/// <summary>
/// Фильтр по дате выпуска фильма.
/// </summary>
public class FilmDateSpec: Specification<Film>
{
    /// <summary>
    /// Фильтр, который отбирает фильмы, которые были выпущены с
    /// <paramref name="firstYear"/> по <paramref name="lastYear"/>.
    /// Включая границы.
    /// </summary>
    public FilmDateSpec(int firstYear, int lastYear)
    {
        Conditional = film => firstYear <= film.Year && film.Year <= lastYear;
    }

    /// <summary>
    /// Фильтр, который отбирает фильмы, которые были выпущены в год <paramref name="year"/>.
    /// </summary>
    /// <param name="year">Год выпуска фильма.</param>
    public FilmDateSpec(int year)
    {
        Conditional = film => film.Year == year;
    }
}