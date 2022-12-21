using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;

/// <summary>
/// Фильтр на получение фильмов
/// </summary>
public class FilmByNamePartSpec : Specification<Film>
{
    /// <summary>
    /// Создает фильтр для получения фильмов по части названия
    /// </summary>
    /// <param name="namePart"></param>
    public FilmByNamePartSpec(string? namePart) =>
        Conditional = f => namePart != null && f.Title.ToLower().Contains(namePart.ToLower());
}