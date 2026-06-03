using PassportOffice.Application.Contracts.Departments;

namespace PassportOffice.Application.Contracts.Services;

public interface IDepartmentsApplicationService
{
    Task<IReadOnlyCollection<DepartmentModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<DepartmentModel?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}

