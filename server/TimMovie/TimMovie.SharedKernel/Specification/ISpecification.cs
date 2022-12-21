namespace TimMovie.SharedKernel.Specification;

public interface ISpecification<TEntity>
{
    bool IsSatisfiedBy(TEntity entity);
}