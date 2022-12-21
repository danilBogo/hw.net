using System.Linq.Expressions;

namespace TimMovie.SharedKernel.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Query { get; }
    Task<TEntity?> AddAsync(TEntity item);
    Task<List<TEntity>> GetAllAsync();
    void Update(TEntity item);
    void Delete(TEntity item);
    public Task SaveChangesAsync();

    IQueryable<TEntity> Include<TProperty>(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, TProperty>> navigationPathToProperty);

    IQueryable<TEntity> ThenInclude<TIncludableEntity, TProperty>
        (IQueryable<TEntity> query, Expression<Func<TIncludableEntity, TProperty>> navigationPathToProperty);
    
    IQueryable<TEntity> ThenIncludeEnumerable<TIncludableEntity, TProperty>
        (IQueryable<TEntity> query, Expression<Func<TIncludableEntity, TProperty>> navigationPathToProperty);

}