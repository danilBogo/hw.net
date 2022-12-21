using System.Linq.Expressions;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Query;

public class IncludableEnumerableQueryExecutor<TEntity, TIncludableProperty> : QueryExecutor<TEntity>
    where TEntity : class
{
    public IncludableEnumerableQueryExecutor(IQueryable<TEntity> query, IRepository<TEntity> repository) 
        : base(query, repository)
    {
    }
    
    public IncludableQueryExecutor<TEntity, TProperty> ThenIncludeInResult<TProperty>(
        Expression<Func<TIncludableProperty,TProperty>> navigationPathToProperty)
    {
        Query = Repository.ThenIncludeEnumerable(Query, navigationPathToProperty);
        
        return new IncludableQueryExecutor<TEntity,TProperty>(Query, Repository);
    }
    
    public IncludableEnumerableQueryExecutor<TEntity, TProperty> ThenIncludeEnumerableInResult<TProperty>(
        Expression<Func<TIncludableProperty,IEnumerable<TProperty>>> navigationPathToProperty)
    {
        Query = Repository.ThenIncludeEnumerable(Query, navigationPathToProperty);
        
        return new IncludableEnumerableQueryExecutor<TEntity,TProperty>(Query, Repository);
    }
}