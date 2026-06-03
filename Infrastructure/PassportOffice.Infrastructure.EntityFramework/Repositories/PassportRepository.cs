using Microsoft.EntityFrameworkCore;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Repositories.Abstractions;

namespace PassportOffice.Infrastructure.EntityFramework.Repositories;

public class PassportRepository(ApplicationDbContext context)
    : EfRepository<Passport, Guid>(context), IPassportRepository
{
    protected override IQueryable<Passport> Query(bool asNoTracking = false)
    {
        var query = Context.Passports
            .Include(passport => passport.Citizen)
            .AsQueryable();

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<Passport?> GetActiveByCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default)
        => await Query()
            .Where(passport => passport.CitizenId == citizenId && passport.Status == PassportStatus.Active)
            .OrderByDescending(passport => passport.IssuedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Passport?> GetBySeriesAndNumberAsync(string series, string number, CancellationToken cancellationToken = default)
    {
        var normalizedSeries = series.Trim();
        var normalizedNumber = number.Trim();
        return await Query()
            .FirstOrDefaultAsync(passport => passport.Series.Value == normalizedSeries && passport.Number.Value == normalizedNumber, cancellationToken);
    }
}

