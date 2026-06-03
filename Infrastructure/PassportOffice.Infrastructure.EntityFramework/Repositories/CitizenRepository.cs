using Microsoft.EntityFrameworkCore;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions;

namespace PassportOffice.Infrastructure.EntityFramework.Repositories;

public class CitizenRepository(ApplicationDbContext context)
    : EfRepository<Citizen, Guid>(context), ICitizenRepository
{
    protected override IQueryable<Citizen> Query(bool asNoTracking = false)
    {
        var query = Context.Citizens
            .Include(citizen => citizen.Passports)
            .Include(citizen => citizen.Registrations)
            .Include(citizen => citizen.Applications)
            .Include(citizen => citizen.Appointments)
            .AsQueryable();

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<Citizen?> GetByPassportAsync(string series, string number, CancellationToken cancellationToken = default)
    {
        var normalizedSeries = series.Trim();
        var normalizedNumber = number.Trim();

        return await Query()
            .FirstOrDefaultAsync(citizen => citizen.Passports.Any(passport =>
                passport.Series.Value == normalizedSeries && passport.Number.Value == normalizedNumber), cancellationToken);
    }

    public async Task<IReadOnlyCollection<Citizen>> SearchByNameAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetAllAsync(cancellationToken, true);

        var normalized = query.Trim();
        return await Query(true)
            .Where(citizen => citizen.FullName.Value.Contains(normalized))
            .ToArrayAsync(cancellationToken);
    }
}

