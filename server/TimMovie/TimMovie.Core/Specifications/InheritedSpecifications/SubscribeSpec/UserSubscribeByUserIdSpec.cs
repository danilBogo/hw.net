using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.SubscribeSpec;

public class UserSubscribeByUserIdSpec: Specification<UserSubscribe>
{
    public UserSubscribeByUserIdSpec(Guid? userId)
    {
        Conditional = subscribe => subscribe.SubscribedUser.Id == userId;
    }
}