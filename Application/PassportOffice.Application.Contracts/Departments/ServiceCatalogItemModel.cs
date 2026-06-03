using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Departments;

public record ServiceCatalogItemModel(
    Guid Id,
    Guid DepartmentId,
    string Code,
    string Name,
    ApplicationType ApplicationType,
    decimal StateFee,
    int ProcessingDays,
    bool IsActive) : IModel<Guid>;

