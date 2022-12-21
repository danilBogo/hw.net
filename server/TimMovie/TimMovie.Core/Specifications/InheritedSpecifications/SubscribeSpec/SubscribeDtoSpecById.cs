
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.SubscribeSpec;

public class SubscribeDtoSpecById: Specification<Subscribe>
{
    public SubscribeDtoSpecById(Guid subscribeId) =>
        Conditional = subscribe => subscribe.Id == subscribeId;
}