using PassportOffice.Domain.Base;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;
using PassportOffice.ValueObjects;

namespace PassportOffice.Domain.Entities;

public class AddressRegistration : Entity<Guid>
{
    public Guid CitizenId { get; private set; }

    public Citizen Citizen { get; private set; } = default!;

    public AddressLine Address { get; private set; } = default!;

    public RegistrationType Type { get; private set; }

    public RegistrationStatus Status { get; private set; }

    public DateOnly RegisteredAt { get; private set; }

    public DateOnly ValidFrom { get; private set; }

    public DateOnly? ValidTo { get; private set; }

    public string Comment { get; private set; } = string.Empty;

    protected AddressRegistration()
    {
    }

    public AddressRegistration(
        Guid id,
        Citizen citizen,
        AddressLine address,
        RegistrationType type,
        DateOnly registeredAt,
        DateOnly validFrom,
        DateOnly? validTo = null,
        string? comment = null) : base(id)
    {
        Citizen = citizen ?? throw new ArgumentNullValueException(nameof(citizen));
        CitizenId = citizen.Id;
        Address = address ?? throw new ArgumentNullValueException(nameof(address));
        Type = type;
        RegisteredAt = registeredAt;
        ValidFrom = validFrom;
        ValidTo = validTo;
        Status = RegistrationStatus.Active;
        Comment = comment?.Trim() ?? string.Empty;

        ValidatePeriod(type, validFrom, validTo);
    }

    public bool IsActiveOn(DateOnly date)
        => Status == RegistrationStatus.Active
            && ValidFrom <= date
            && (ValidTo is null || ValidTo >= date);

    public void Close(DateOnly closedAt, string? comment = null)
    {
        if (closedAt < ValidFrom)
            throw new RegistrationPeriodException("Дата снятия с регистрации не может быть раньше даты постановки.");

        if (Status == RegistrationStatus.Closed)
            return;

        Status = RegistrationStatus.Closed;
        ValidTo = closedAt;
        Comment = comment?.Trim() ?? Comment;
    }

    public void Cancel(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new RegistrationPeriodException("Для отмены регистрации нужно указать причину.");

        Status = RegistrationStatus.Cancelled;
        Comment = reason.Trim();
    }

    private static void ValidatePeriod(RegistrationType type, DateOnly validFrom, DateOnly? validTo)
    {
        if (validTo is not null && validTo <= validFrom)
            throw new RegistrationPeriodException("Дата окончания регистрации должна быть позже даты начала.");

        if (type == RegistrationType.Temporary && validTo is null)
            throw new RegistrationPeriodException("Для временной регистрации обязательна дата окончания.");
    }
}
