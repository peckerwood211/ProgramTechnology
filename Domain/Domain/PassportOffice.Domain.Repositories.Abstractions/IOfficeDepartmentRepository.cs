using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions.Base;

namespace PassportOffice.Domain.Repositories.Abstractions;

public interface IOfficeDepartmentRepository : IRepository<OfficeDepartment, Guid>
{
    Task<OfficeDepartment?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}

