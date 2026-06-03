using PassportOffice.Application.Contracts.Base;

namespace PassportOffice.Application.Contracts.Departments;

public record DepartmentModel(
    Guid Id,
    string Code,
    string Name,
    string Address,
    string WorkingHours,
    IReadOnlyCollection<EmployeeModel> Employees,
    IReadOnlyCollection<ServiceCatalogItemModel> Services) : IModel<Guid>;

