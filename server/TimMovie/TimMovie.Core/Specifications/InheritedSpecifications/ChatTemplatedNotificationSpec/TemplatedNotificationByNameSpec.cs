using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.ChatTemplatedNotificationSpec;

public class TemplatedNotificationByNameSpec: Specification<ChatTemplatedNotification>
{
    public TemplatedNotificationByNameSpec(string name)
    {
        Conditional = notification => notification.Name == name;
    }
}