using Microsoft.EntityFrameworkCore;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Repositories.Abstractions;

namespace PassportOffice.Infrastructure.EntityFramework.Repositories;

public class PassportApplicationRepository(ApplicationDbContext context)
    : EfRepository<PassportApplication, Guid>(context), IPassportApplicationRepository
{
    protected override IQueryable<PassportApplication> Query(bool asNoTracking = false)
    {
        var query = Context.PassportApplications
            .Include(application => application.Citizen)
            .Include(application => application.Service)
            .Include(application => application.Department)
            .Include(application => application.Employee)
            .Include(application => application.Documents)
            .AsQueryable();

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<PassportApplication?> GetByNumberAsync(string number, CancellationToken cancellationToken = default)
        => await Query().FirstOrDefaultAsync(application => application.Number == number.Trim(), cancellationToken);

    public async Task<IReadOnlyCollection<PassportApplication>> GetByStatusAsync(ApplicationStatus status, CancellationToken cancellationToken = default)
        => await Query(true)
            .Where(application => application.Status == status)
            .OrderBy(application => application.SubmittedAt)
            .ToArrayAsync(cancellationToken);
}

