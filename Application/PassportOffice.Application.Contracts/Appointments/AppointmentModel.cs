using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Appointments;

public record AppointmentModel(
    Guid Id,
    Guid CitizenId,
    string CitizenFullName,
    Guid DepartmentId,
    string DepartmentCode,
    Guid? EmployeeId,
    string? EmployeeFullName,
    Guid? ApplicationId,
    DateTime ScheduledAt,
    AppointmentStatus Status,
    string Purpose) : IModel<Guid>;

