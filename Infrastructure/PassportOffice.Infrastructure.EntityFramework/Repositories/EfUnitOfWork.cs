using PassportOffice.Domain.Repositories.Abstractions;

namespace PassportOffice.Infrastructure.EntityFramework.Repositories;

public class EfUnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}

