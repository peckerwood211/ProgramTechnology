using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Repositories.Abstractions.Base;

namespace PassportOffice.Domain.Repositories.Abstractions;

public interface IPassportApplicationRepository : IRepository<PassportApplication, Guid>
{
    Task<PassportApplication?> GetByNumberAsync(string number, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PassportApplication>> GetByStatusAsync(ApplicationStatus status, CancellationToken cancellationToken = default);
}

