using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.SubscribeSpec;

/// <summary>
/// Фильтр на получение подписок
/// </summary>
public class UserSubscribeByNamePartSpec : Specification<Subscribe>
{
    /// <summary>
    /// Создает фильтр для получения подписок по части названия
    /// При пустой части названия фильтрация не производится
    /// </summary>
    /// <param name="namePart"></param>
    public UserSubscribeByNamePartSpec(string? namePart) =>
        Conditional = s => namePart == null || s.Name.ToLower().Contains(namePart.ToLower());
}