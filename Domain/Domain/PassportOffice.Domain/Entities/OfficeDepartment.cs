using PassportOffice.Domain.Base;
using PassportOffice.Domain.Exceptions;
using PassportOffice.ValueObjects;

namespace PassportOffice.Domain.Entities;

public class OfficeDepartment : Entity<Guid>, IAggregateRoot
{
    private readonly List<OfficeEmployee> _employees = new();
    private readonly List<ServiceCatalogItem> _services = new();

    public DepartmentCode Code { get; private set; } = default!;

    public string Name { get; private set; } = string.Empty;

    public AddressLine Address { get; private set; } = default!;

    public string WorkingHours { get; private set; } = string.Empty;

    public IReadOnlyCollection<OfficeEmployee> Employees => _employees.AsReadOnly();

    public IReadOnlyCollection<ServiceCatalogItem> Services => _services.AsReadOnly();

    protected OfficeDepartment()
    {
    }

    public OfficeDepartment(
        Guid id,
        DepartmentCode code,
        string name,
        AddressLine address,
        string workingHours) : base(id)
    {
        Code = code ?? throw new ArgumentNullValueException(nameof(code));
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Название подразделения обязательно.", nameof(name)) : name.Trim();
        Address = address ?? throw new ArgumentNullValueException(nameof(address));
        WorkingHours = string.IsNullOrWhiteSpace(workingHours) ? "Пн-Пт 09:00-18:00" : workingHours.Trim();
    }

    public OfficeEmployee AddEmployee(FullName fullName, Enums.EmployeePosition position, string personnelNumber)
    {
        if (_employees.Any(employee => employee.PersonnelNumber.Equals(personnelNumber, StringComparison.OrdinalIgnoreCase)))
            throw new ApplicationStateException("Сотрудник с таким табельным номером уже есть в подразделении.");

        var employee = new OfficeEmployee(Guid.NewGuid(), this, fullName, position, personnelNumber);
        _employees.Add(employee);
        return employee;
    }

    public ServiceCatalogItem AddService(
        ServiceCode code,
        string name,
        Enums.ApplicationType applicationType,
        decimal stateFee,
        int processingDays)
    {
        if (_services.Any(service => service.Code == code))
            throw new ApplicationStateException("Услуга с таким кодом уже есть в подразделении.");

        var serviceItem = new ServiceCatalogItem(Guid.NewGuid(), this, code, name, applicationType, stateFee, processingDays);
        _services.Add(serviceItem);
        return serviceItem;
    }
}

