using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions.Base;

namespace PassportOffice.Domain.Repositories.Abstractions;

public interface ICitizenRepository : IRepository<Citizen, Guid>
{
    Task<Citizen?> GetByPassportAsync(string series, string number, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Citizen>> SearchByNameAsync(string query, CancellationToken cancellationToken = default);
}

