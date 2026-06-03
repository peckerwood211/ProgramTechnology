using PassportOffice.Application.Contracts.Departments;
using PassportOffice.Application.Contracts.Services;
using PassportOffice.Application.Services.Mapping;
using PassportOffice.Domain.Repositories.Abstractions;

namespace PassportOffice.Application.Services;

public class DepartmentsApplicationService(IOfficeDepartmentRepository departments) : IDepartmentsApplicationService
{
    public async Task<IReadOnlyCollection<DepartmentModel>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await departments.GetAllAsync(cancellationToken, true))
            .Select(department => department.ToModel())
            .ToArray();

    public async Task<DepartmentModel?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => (await departments.GetByCodeAsync(code, cancellationToken))?.ToModel();
}

