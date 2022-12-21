using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.GenreSpec;

/// <summary>
/// Фильтр на получение жанров
/// </summary>
public class GenreByNamePartSpec : Specification<Genre>
{
    /// <summary>
    /// Создает фильтр для получения жанров по части названия
    /// </summary>
    /// <param name="namePart"></param>
    public GenreByNamePartSpec(string? namePart) =>
        Conditional = g => namePart != null && g.Name.ToLower().Contains(namePart.ToLower());
}