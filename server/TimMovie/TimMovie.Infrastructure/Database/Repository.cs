using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Infrastructure.Database;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> Query => _dbSet.AsQueryable();

    public async Task<List<TEntity>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<TEntity?> AddAsync(TEntity item)
    {
        var entityEntry = await _dbSet.AddAsync(item);
        return entityEntry.Entity;
    }

    public void Update(TEntity item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(TEntity item)
    {
        _dbSet.Remove(item);
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public IQueryable<TEntity> Include<TProperty>(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, TProperty>> navigationPathToProperty)
    {
        return query.Include(navigationPathToProperty);
    }

    public IQueryable<TEntity> ThenInclude<TIncludableEntity, TProperty>
        (IQueryable<TEntity> query, Expression<Func<TIncludableEntity, TProperty>> navigationPathToProperty)
    {
        var includable = query as IIncludableQueryable<TEntity, TIncludableEntity>;
        if (includable is null)
        {
            throw new InvalidOperationException(
                $"{nameof(ThenInclude)} был применен к запросу, к которуму не был применен {nameof(Include)}");
        }

        return includable.ThenInclude(navigationPathToProperty);
    }
    
    public IQueryable<TEntity> ThenIncludeEnumerable<TIncludableEntity, TProperty>
        (IQueryable<TEntity> query, Expression<Func<TIncludableEntity, TProperty>> navigationPathToProperty)
    {
        var includable = query as IIncludableQueryable<TEntity, IEnumerable<TIncludableEntity>>;
        if (includable is null)
        {
            throw new InvalidOperationException(
                $"{nameof(ThenIncludeEnumerable)} был применен к запросу, к которуму не был применен {nameof(Include)}." +
                $"Либо при {nameof(Include)} не был {nameof(IEnumerable<TIncludableEntity>)}.");
        }

        return includable.ThenInclude(navigationPathToProperty);
    }
}