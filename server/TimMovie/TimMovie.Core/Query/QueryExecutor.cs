using System.Linq.Expressions;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Query;

public class QueryExecutor<TEntity>
    where TEntity: class
{
    protected IQueryable<TEntity> Query;
    protected readonly IRepository<TEntity> Repository;

    public QueryExecutor(IQueryable<TEntity> query, IRepository<TEntity> repository)
    {
        Query = query;
        Repository = repository;
    }

    public IEnumerable<TEntity> GetEntitiesWithPagination(int amountSkip, int amountTake)
    {
        return Query
            .Skip(amountSkip)
            .Take(amountTake)
            .ToList();
    }
    
    public IEnumerable<TEntity> GetEntities()
    {
        return Query.ToList();
    }

    public TEntity? FirstOrDefault()
    {
        return Query
            .FirstOrDefault();
    }

    public IncludableQueryExecutor<TEntity,TProperty> IncludeInResult<TProperty>(
        Expression<Func<TEntity, TProperty>> navigationPathToProperty)
    {
        Query = Repository.Include(Query, navigationPathToProperty);
        
        return new IncludableQueryExecutor<TEntity,TProperty>(Query, Repository);
    }
    
    public IncludableEnumerableQueryExecutor<TEntity, TProperty> IncludeEnumerableInResult<TProperty>(
        Expression<Func<TEntity, IEnumerable<TProperty>>> navigationPathToProperty)
    {
        Query = Repository.Include(Query, navigationPathToProperty);

        return new IncludableEnumerableQueryExecutor<TEntity, TProperty>(Query, Repository);
    }
}