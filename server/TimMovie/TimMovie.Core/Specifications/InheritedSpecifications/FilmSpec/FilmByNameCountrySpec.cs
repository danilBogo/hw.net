using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;

/// <summary>
/// Фильтр по названию страны фильма.
/// </summary>
public class FilmByNameCountrySpec: Specification<Film>
{
    /// <summary>
    /// Филтьр, который отбирает фильмы только с названием страны <paramref name="countryName"/>.
    /// </summary>
    /// <param name="countryName"></param>
    public FilmByNameCountrySpec(string countryName)
    {
        Conditional = film => film.Country!.Name == countryName;
    }
}