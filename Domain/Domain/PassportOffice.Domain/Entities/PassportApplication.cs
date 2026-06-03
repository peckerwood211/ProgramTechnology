using PassportOffice.Domain.Base;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;

namespace PassportOffice.Domain.Entities;

public class PassportApplication : Entity<Guid>, IAggregateRoot
{
    private readonly List<AttachedDocument> _documents = new();

    public string Number { get; private set; } = string.Empty;

    public Guid CitizenId { get; private set; }

    public Citizen Citizen { get; private set; } = default!;

    public Guid ServiceId { get; private set; }

    public ServiceCatalogItem Service { get; private set; } = default!;

    public Guid DepartmentId { get; private set; }

    public OfficeDepartment Department { get; private set; } = default!;

    public Guid? EmployeeId { get; private set; }

    public OfficeEmployee? Employee { get; private set; }

    public ApplicationType Type { get; private set; }

    public ApplicationStatus Status { get; private set; }

    public DateTime SubmittedAt { get; private set; }

    public DateTime? AcceptedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public string Comment { get; private set; } = string.Empty;

    public IReadOnlyCollection<AttachedDocument> Documents => _documents.AsReadOnly();

    protected PassportApplication()
    {
    }

    public PassportApplication(
        Guid id,
        Citizen citizen,
        ServiceCatalogItem service,
        OfficeDepartment department,
        ApplicationType type,
        DateTime submittedAt,
        string? comment = null) : base(id)
    {
        Citizen = citizen ?? throw new ArgumentNullValueException(nameof(citizen));
        CitizenId = citizen.Id;
        Service = service ?? throw new ArgumentNullValueException(nameof(service));
        ServiceId = service.Id;
        Department = department ?? throw new ArgumentNullValueException(nameof(department));
        DepartmentId = department.Id;
        Type = type;
        SubmittedAt = submittedAt;
        Status = ApplicationStatus.Submitted;
        Number = GenerateNumber(submittedAt);
        Comment = comment?.Trim() ?? string.Empty;
    }

    public AttachedDocument AttachDocument(AttachedDocumentType type, string name, string? number, DateOnly receivedAt)
    {
        if (Status is ApplicationStatus.Approved or ApplicationStatus.Rejected or ApplicationStatus.Cancelled)
            throw new ApplicationStateException("Нельзя добавлять документы в закрытое заявление.");

        var document = new AttachedDocument(Guid.NewGuid(), this, type, name, number, receivedAt);
        _documents.Add(document);
        return document;
    }

    public void Accept(OfficeEmployee employee, DateTime acceptedAt)
    {
        if (Status != ApplicationStatus.Submitted)
            throw new ApplicationStateException("Принять можно только поданное заявление.");

        AssignEmployee(employee);
        AcceptedAt = acceptedAt;
        Status = ApplicationStatus.Accepted;
    }

    public void AssignEmployee(OfficeEmployee employee)
    {
        Employee = employee ?? throw new ArgumentNullValueException(nameof(employee));
        EmployeeId = employee.Id;

        if (Status == ApplicationStatus.Accepted)
            Status = ApplicationStatus.InProgress;
    }

    public void Approve(OfficeEmployee employee, string? comment, DateTime completedAt)
    {
        if (Status is ApplicationStatus.Approved or ApplicationStatus.Rejected or ApplicationStatus.Cancelled)
            throw new ApplicationStateException("Заявление уже закрыто.");

        AssignEmployee(employee);
        Status = ApplicationStatus.Approved;
        CompletedAt = completedAt;
        Comment = comment?.Trim() ?? Comment;
    }

    public void Reject(OfficeEmployee employee, string reason, DateTime completedAt)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ApplicationStateException("Для отказа нужно указать причину.");

        if (Status is ApplicationStatus.Approved or ApplicationStatus.Rejected or ApplicationStatus.Cancelled)
            throw new ApplicationStateException("Заявление уже закрыто.");

        AssignEmployee(employee);
        Status = ApplicationStatus.Rejected;
        CompletedAt = completedAt;
        Comment = reason.Trim();
    }

    public void Cancel(string reason)
    {
        if (Status is ApplicationStatus.Approved or ApplicationStatus.Rejected)
            throw new ApplicationStateException("Нельзя отменить рассмотренное заявление.");

        Status = ApplicationStatus.Cancelled;
        Comment = reason.Trim();
    }

    private static string GenerateNumber(DateTime submittedAt)
        => $"PO-{submittedAt:yyyyMMdd}-{Guid.NewGuid():N}"[..24].ToUpperInvariant();
}
