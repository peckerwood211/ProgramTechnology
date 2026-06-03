using PassportOffice.Domain.Base;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;
using PassportOffice.ValueObjects;

namespace PassportOffice.Domain.Entities;

public class OfficeEmployee : Entity<Guid>
{
    public Guid DepartmentId { get; private set; }

    public OfficeDepartment Department { get; private set; } = default!;

    public FullName FullName { get; private set; } = default!;

    public EmployeePosition Position { get; private set; }

    public string PersonnelNumber { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    protected OfficeEmployee()
    {
    }

    public OfficeEmployee(
        Guid id,
        OfficeDepartment department,
        FullName fullName,
        EmployeePosition position,
        string personnelNumber) : base(id)
    {
        Department = department ?? throw new ArgumentNullValueException(nameof(department));
        DepartmentId = department.Id;
        FullName = fullName ?? throw new ArgumentNullValueException(nameof(fullName));
        Position = position;
        PersonnelNumber = string.IsNullOrWhiteSpace(personnelNumber)
            ? throw new ArgumentException("Табельный номер обязателен.", nameof(personnelNumber))
            : personnelNumber.Trim();
        IsActive = true;
    }

    public void ChangePosition(EmployeePosition position)
        => Position = position;

    public void Deactivate()
        => IsActive = false;
}

