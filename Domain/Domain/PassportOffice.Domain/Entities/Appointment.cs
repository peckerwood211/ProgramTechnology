using PassportOffice.Domain.Base;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;

namespace PassportOffice.Domain.Entities;

public class Appointment : Entity<Guid>
{
    public Guid CitizenId { get; private set; }

    public Citizen Citizen { get; private set; } = default!;

    public Guid DepartmentId { get; private set; }

    public OfficeDepartment Department { get; private set; } = default!;

    public Guid? EmployeeId { get; private set; }

    public OfficeEmployee? Employee { get; private set; }

    public Guid? ApplicationId { get; private set; }

    public PassportApplication? Application { get; private set; }

    public DateTime ScheduledAt { get; private set; }

    public AppointmentStatus Status { get; private set; }

    public string Purpose { get; private set; } = string.Empty;

    protected Appointment()
    {
    }

    public Appointment(
        Guid id,
        Citizen citizen,
        OfficeDepartment department,
        DateTime scheduledAt,
        string purpose,
        OfficeEmployee? employee = null,
        PassportApplication? application = null) : base(id)
    {
        Citizen = citizen ?? throw new ArgumentNullValueException(nameof(citizen));
        CitizenId = citizen.Id;
        Department = department ?? throw new ArgumentNullValueException(nameof(department));
        DepartmentId = department.Id;
        Employee = employee;
        EmployeeId = employee?.Id;
        Application = application;
        ApplicationId = application?.Id;
        ScheduledAt = scheduledAt;
        Purpose = string.IsNullOrWhiteSpace(purpose) ? "Прием гражданина" : purpose.Trim();
        Status = AppointmentStatus.Planned;

        ValidateScheduledAt(scheduledAt);
    }

    public void Confirm()
    {
        if (Status != AppointmentStatus.Planned)
            throw new AppointmentTimeException("Подтвердить можно только запланированный прием.");

        Status = AppointmentStatus.Confirmed;
    }

    public void Complete()
    {
        if (Status is AppointmentStatus.Cancelled or AppointmentStatus.Missed)
            throw new AppointmentTimeException("Нельзя завершить отмененный или пропущенный прием.");

        Status = AppointmentStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Completed)
            throw new AppointmentTimeException("Нельзя отменить завершенный прием.");

        Status = AppointmentStatus.Cancelled;
    }

    public void Move(DateTime newScheduledAt)
    {
        ValidateScheduledAt(newScheduledAt);
        ScheduledAt = newScheduledAt;
        Status = AppointmentStatus.Planned;
    }

    private static void ValidateScheduledAt(DateTime scheduledAt)
    {
        if (scheduledAt <= DateTime.UtcNow.AddMinutes(5))
            throw new AppointmentTimeException("Дата приема должна быть в будущем.");
    }
}
