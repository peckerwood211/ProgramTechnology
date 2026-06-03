using PassportOffice.Application.Contracts.Applications;
using PassportOffice.Application.Contracts.Appointments;
using PassportOffice.Application.Contracts.Citizens;
using PassportOffice.Application.Contracts.Departments;
using PassportOffice.Application.Contracts.Passports;
using PassportOffice.Domain.Entities;

namespace PassportOffice.Application.Services.Mapping;

internal static class DomainMappingExtensions
{
    public static CitizenModel ToModel(this Citizen citizen)
        => new(
            citizen.Id,
            citizen.FullName.Value,
            citizen.BirthDate,
            citizen.BirthPlace,
            citizen.Gender,
            citizen.Snils,
            citizen.Phone?.Value,
            citizen.Email?.Value,
            citizen.CurrentPassport?.FullNumber,
            citizen.CurrentPermanentRegistration?.Address.Value,
            citizen.Applications.Count);

    public static PassportModel ToModel(this Passport passport)
        => new(
            passport.Id,
            passport.CitizenId,
            passport.Series.Value,
            passport.Number.Value,
            passport.FullNumber,
            passport.DepartmentCode.Value,
            passport.IssuedBy,
            passport.IssuedAt,
            passport.ExpiresAt,
            passport.Type,
            passport.Status);

    public static AddressRegistrationModel ToModel(this AddressRegistration registration)
        => new(
            registration.Id,
            registration.CitizenId,
            registration.Address.Value,
            registration.Type,
            registration.Status,
            registration.RegisteredAt,
            registration.ValidFrom,
            registration.ValidTo,
            registration.Comment);

    public static PassportApplicationModel ToModel(this PassportApplication application)
        => new(
            application.Id,
            application.Number,
            application.CitizenId,
            application.Citizen.FullName.Value,
            application.DepartmentId,
            application.Department.Code.Value,
            application.Service.Code.Value,
            application.Type,
            application.Status,
            application.SubmittedAt,
            application.AcceptedAt,
            application.CompletedAt,
            application.EmployeeId,
            application.Employee?.FullName.Value,
            application.Comment,
            application.Documents.Select(document => document.ToModel()).ToArray());

    public static AttachedDocumentModel ToModel(this AttachedDocument document)
        => new(
            document.Id,
            document.ApplicationId,
            document.Type,
            document.Name,
            document.Number,
            document.ReceivedAt);

    public static AppointmentModel ToModel(this Appointment appointment)
        => new(
            appointment.Id,
            appointment.CitizenId,
            appointment.Citizen.FullName.Value,
            appointment.DepartmentId,
            appointment.Department.Code.Value,
            appointment.EmployeeId,
            appointment.Employee?.FullName.Value,
            appointment.ApplicationId,
            appointment.ScheduledAt,
            appointment.Status,
            appointment.Purpose);

    public static DepartmentModel ToModel(this OfficeDepartment department)
        => new(
            department.Id,
            department.Code.Value,
            department.Name,
            department.Address.Value,
            department.WorkingHours,
            department.Employees.Select(employee => employee.ToModel()).ToArray(),
            department.Services.Select(service => service.ToModel()).ToArray());

    public static EmployeeModel ToModel(this OfficeEmployee employee)
        => new(
            employee.Id,
            employee.DepartmentId,
            employee.FullName.Value,
            employee.Position,
            employee.PersonnelNumber,
            employee.IsActive);

    public static ServiceCatalogItemModel ToModel(this ServiceCatalogItem service)
        => new(
            service.Id,
            service.DepartmentId,
            service.Code.Value,
            service.Name,
            service.ApplicationType,
            service.StateFee,
            service.ProcessingDays,
            service.IsActive);
}

