using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.SharedKernel.BaseEntities;

public abstract class BaseEntity: IIdHolder<Guid>
{
    public Guid Id { get; set; }
}