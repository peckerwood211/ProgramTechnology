using PassportOffice.Application.Contracts.Appointments;
using PassportOffice.Application.Contracts.Services;
using PassportOffice.Application.Services.Mapping;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;
using PassportOffice.Domain.Repositories.Abstractions;
using PassportOffice.Domain.Repositories.Abstractions.Base;

namespace PassportOffice.Application.Services;

public class AppointmentsApplicationService(
    IAppointmentRepository appointments,
    ICitizenRepository citizens,
    IOfficeDepartmentRepository departments,
    IRepository<OfficeEmployee, Guid> employees,
    IPassportApplicationRepository applications,
    IUnitOfWork unitOfWork) : IAppointmentsApplicationService
{
    public async Task<IReadOnlyCollection<AppointmentModel>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await appointments.GetAllAsync(cancellationToken, true))
            .Select(appointment => appointment.ToModel())
            .ToArray();

    public async Task<AppointmentModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await appointments.GetByIdAsync(id, cancellationToken))?.ToModel();

    public async Task<IReadOnlyCollection<AppointmentModel>> GetByCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default)
        => (await appointments.GetByCitizenAsync(citizenId, cancellationToken))
            .Select(appointment => appointment.ToModel())
            .ToArray();

    public async Task<AppointmentModel?> CreateAsync(CreateAppointmentModel model, CancellationToken cancellationToken = default)
    {
        var citizen = await citizens.GetByIdAsync(model.CitizenId, cancellationToken);
        if (citizen is null)
            return null;

        var department = await departments.GetByCodeAsync(model.DepartmentCode, cancellationToken);
        if (department is null)
            return null;

        OfficeEmployee? employee = null;
        if (model.EmployeeId is not null)
        {
            employee = await employees.GetByIdAsync(model.EmployeeId.Value, cancellationToken);
            if (employee is null)
                return null;
        }

        PassportApplication? application = null;
        if (model.ApplicationId is not null)
        {
            application = await applications.GetByIdAsync(model.ApplicationId.Value, cancellationToken);
            if (application is null)
                return null;
        }

        await EnsureSlotAvailableAsync(department.Id, employee?.Id, model.ScheduledAt, cancellationToken);

        var appointment = citizen.BookAppointment(
            department,
            DateTime.SpecifyKind(model.ScheduledAt, DateTimeKind.Utc),
            model.Purpose,
            employee,
            application);

        await appointments.AddAsync(appointment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return appointment.ToModel();
    }

    public Task<bool> ConfirmAsync(Guid id, CancellationToken cancellationToken = default)
        => ChangeStatusAsync(id, appointment => appointment.Confirm(), cancellationToken);

    public Task<bool> CompleteAsync(Guid id, CancellationToken cancellationToken = default)
        => ChangeStatusAsync(id, appointment => appointment.Complete(), cancellationToken);

    public Task<bool> CancelAsync(Guid id, CancellationToken cancellationToken = default)
        => ChangeStatusAsync(id, appointment => appointment.Cancel(), cancellationToken);

    private async Task<bool> ChangeStatusAsync(Guid id, Action<Appointment> action, CancellationToken cancellationToken)
    {
        var appointment = await appointments.GetByIdAsync(id, cancellationToken);
        if (appointment is null)
            return false;

        action(appointment);
        await appointments.UpdateAsync(appointment, cancellationToken);
        return await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task EnsureSlotAvailableAsync(Guid departmentId, Guid? employeeId, DateTime scheduledAt, CancellationToken cancellationToken)
    {
        var existing = await appointments.GetForDateAsync(departmentId, DateOnly.FromDateTime(scheduledAt), cancellationToken);
        var busy = existing.Any(appointment =>
            appointment.Status is not (AppointmentStatus.Cancelled or AppointmentStatus.Completed or AppointmentStatus.Missed)
            && appointment.ScheduledAt == scheduledAt
            && (employeeId is null || appointment.EmployeeId == employeeId));

        if (busy)
            throw new AppointmentTimeException("На это время уже есть запись на это время.");
    }
}

