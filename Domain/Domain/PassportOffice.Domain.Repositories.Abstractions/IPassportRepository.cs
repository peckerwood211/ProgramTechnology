using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions.Base;

namespace PassportOffice.Domain.Repositories.Abstractions;

public interface IPassportRepository : IRepository<Passport, Guid>
{
    Task<Passport?> GetActiveByCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default);

    Task<Passport?> GetBySeriesAndNumberAsync(string series, string number, CancellationToken cancellationToken = default);
}

