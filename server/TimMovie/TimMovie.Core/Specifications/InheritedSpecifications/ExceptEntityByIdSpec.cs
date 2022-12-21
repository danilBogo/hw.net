using TimMovie.SharedKernel.Interfaces;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications;

public class ExceptEntityByIdSpec<TEntity>: Specification<TEntity> 
    where TEntity: IIdHolder<Guid>
{
    public ExceptEntityByIdSpec(Guid id)
    {
        Conditional = entity => entity.Id != id;
    }
}