using PassportOffice.Domain.Base;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;
using PassportOffice.ValueObjects;

namespace PassportOffice.Domain.Entities;

public class ServiceCatalogItem : Entity<Guid>
{
    public Guid DepartmentId { get; private set; }

    public OfficeDepartment Department { get; private set; } = default!;

    public ServiceCode Code { get; private set; } = default!;

    public string Name { get; private set; } = string.Empty;

    public ApplicationType ApplicationType { get; private set; }

    public decimal StateFee { get; private set; }

    public int ProcessingDays { get; private set; }

    public bool IsActive { get; private set; }

    protected ServiceCatalogItem()
    {
    }

    public ServiceCatalogItem(
        Guid id,
        OfficeDepartment department,
        ServiceCode code,
        string name,
        ApplicationType applicationType,
        decimal stateFee,
        int processingDays) : base(id)
    {
        Department = department ?? throw new ArgumentNullValueException(nameof(department));
        DepartmentId = department.Id;
        Code = code ?? throw new ArgumentNullValueException(nameof(code));
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Название услуги обязательно.", nameof(name)) : name.Trim();
        ApplicationType = applicationType;
        StateFee = stateFee < 0 ? throw new ApplicationStateException("Госпошлина не может быть отрицательной.") : stateFee;
        ProcessingDays = processingDays <= 0 ? throw new ApplicationStateException("Срок оказания услуги должен быть положительным.") : processingDays;
        IsActive = true;
    }

    public void Disable()
        => IsActive = false;
}
