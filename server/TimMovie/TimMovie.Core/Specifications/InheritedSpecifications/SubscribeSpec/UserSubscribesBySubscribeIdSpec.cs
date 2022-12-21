using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.SubscribeSpec;

public class UserSubscribesBySubscribeIdSpec: Specification<UserSubscribe>
{
    public UserSubscribesBySubscribeIdSpec(Guid subscribeId)
    {
        Conditional = subscribe => subscribe.Subscribe.Id == subscribeId;
    } 
}