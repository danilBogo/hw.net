using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.ProducerSpec;

/// <summary>
/// Фильтр на получение продюссеров
/// </summary>
public class ProducerByNamePartSpec : Specification<Producer>
{
    /// <summary>
    /// Создает фильтр для получения продюссеров по части имени и фамилии
    /// </summary>
    /// <param name="namePart"></param>
    public ProducerByNamePartSpec(string? namePart) =>
        Conditional = p =>
            namePart != null &&
            p.Surname == null
                ? p.Name.ToLower().Contains(namePart.ToLower())
                : (p.Name.ToLower() + " " + p.Surname!.ToLower()).Contains(namePart!.ToLower());
}