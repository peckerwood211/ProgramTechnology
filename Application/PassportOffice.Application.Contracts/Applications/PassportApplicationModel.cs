using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Applications;

public record PassportApplicationModel(
    Guid Id,
    string Number,
    Guid CitizenId,
    string CitizenFullName,
    Guid DepartmentId,
    string DepartmentCode,
    string ServiceCode,
    ApplicationType Type,
    ApplicationStatus Status,
    DateTime SubmittedAt,
    DateTime? AcceptedAt,
    DateTime? CompletedAt,
    Guid? EmployeeId,
    string? EmployeeFullName,
    string Comment,
    IReadOnlyCollection<AttachedDocumentModel> Documents) : IModel<Guid>;

