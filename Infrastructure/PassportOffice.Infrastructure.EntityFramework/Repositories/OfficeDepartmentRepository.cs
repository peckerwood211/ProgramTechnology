using Microsoft.EntityFrameworkCore;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions;

namespace PassportOffice.Infrastructure.EntityFramework.Repositories;

public class OfficeDepartmentRepository(ApplicationDbContext context)
    : EfRepository<OfficeDepartment, Guid>(context), IOfficeDepartmentRepository
{
    protected override IQueryable<OfficeDepartment> Query(bool asNoTracking = false)
    {
        var query = Context.Departments
            .Include(department => department.Employees)
            .Include(department => department.Services)
            .AsQueryable();

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<OfficeDepartment?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim();
        return await Query().FirstOrDefaultAsync(department => department.Code.Value == normalized, cancellationToken);
    }
}

