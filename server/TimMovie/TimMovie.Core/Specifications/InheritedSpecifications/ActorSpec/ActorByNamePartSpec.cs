using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.ActorSpec;

/// <summary>
/// Фильтр на получение актеро
/// </summary>
public class ActorByNamePartSpec : Specification<Actor>
{
    /// <summary>
    /// Создает фильтр для получения актеров по части имени и фамилии
    /// </summary>
    /// <param name="namePart"></param>
    public ActorByNamePartSpec(string? namePart) =>
        Conditional = p =>
            namePart != null &&
            p.Surname == null
                ? p.Name.ToLower().Contains(namePart.ToLower())
                : (p.Name.ToLower() + " " + p.Surname!.ToLower()).Contains(namePart!.ToLower());
}