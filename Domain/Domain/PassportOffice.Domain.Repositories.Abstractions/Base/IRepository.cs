using PassportOffice.Domain.Base;

namespace PassportOffice.Domain.Repositories.Abstractions.Base;

public interface IRepository<TEntity, in TId>
    where TEntity : Entity<TId>
    where TId : struct, IEquatable<TId>
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default, bool asNoTracking = false);

    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}

