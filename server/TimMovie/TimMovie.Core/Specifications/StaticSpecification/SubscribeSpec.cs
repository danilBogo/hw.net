using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.StaticSpecification;

public class SubscribeSpec
{
    public static readonly Specification<UserSubscribe> ActiveUserSubscribe =
        new(subscribe => subscribe.EndDate > DateTime.Now);
    public static readonly Specification<Subscribe> ActiveSubscribe =
        new(subscribe => subscribe.IsActive);
}