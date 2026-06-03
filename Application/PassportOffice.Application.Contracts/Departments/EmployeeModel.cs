using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Departments;

public record EmployeeModel(
    Guid Id,
    Guid DepartmentId,
    string FullName,
    EmployeePosition Position,
    string PersonnelNumber,
    bool IsActive) : IModel<Guid>;

