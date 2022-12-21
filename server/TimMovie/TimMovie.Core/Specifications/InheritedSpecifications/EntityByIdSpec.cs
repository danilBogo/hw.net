using TimMovie.SharedKernel.Interfaces;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications;

public class EntityByIdSpec<TEntity>: Specification<TEntity> 
    where TEntity: IIdHolder<Guid>
{
    public EntityByIdSpec(Guid id)
    {
        Conditional = entity => entity.Id == id;
    }
}