using Microsoft.EntityFrameworkCore;
using PassportOffice.Domain.Base;
using PassportOffice.Domain.Repositories.Abstractions.Base;

namespace PassportOffice.Infrastructure.EntityFramework.Repositories;

public class EfRepository<TEntity, TId>(ApplicationDbContext context) : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : struct, IEquatable<TId>
{
    protected ApplicationDbContext Context { get; } = context;

    protected virtual IQueryable<TEntity> Query(bool asNoTracking = false)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return asNoTracking ? query.AsNoTracking() : query;
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default, bool asNoTracking = false)
        => await Query(asNoTracking).ToArrayAsync(cancellationToken);

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        => await Query().FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Context.Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }
}

