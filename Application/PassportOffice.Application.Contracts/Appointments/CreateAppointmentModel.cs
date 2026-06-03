using PassportOffice.Application.Contracts.Base;

namespace PassportOffice.Application.Contracts.Appointments;

public record CreateAppointmentModel(
    Guid CitizenId,
    string DepartmentCode,
    DateTime ScheduledAt,
    string Purpose,
    Guid? EmployeeId = null,
    Guid? ApplicationId = null) : ICreateModel;

