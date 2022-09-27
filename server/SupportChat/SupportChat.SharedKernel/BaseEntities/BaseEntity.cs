using SharedKernel.Interfaces;

namespace SharedKernel.BaseEntities;

public class BaseEntity : IIdHolder<Guid>
{
    public Guid Id { get; set; }
}