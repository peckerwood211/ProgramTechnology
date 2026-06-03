using PassportOffice.Domain.Base;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;
using PassportOffice.ValueObjects;

namespace PassportOffice.Domain.Entities;

public class Passport : Entity<Guid>
{
    public Guid CitizenId { get; private set; }

    public Citizen Citizen { get; private set; } = default!;

    public PassportSeries Series { get; private set; } = default!;

    public PassportNumber Number { get; private set; } = default!;

    public DepartmentCode DepartmentCode { get; private set; } = default!;

    public string IssuedBy { get; private set; } = string.Empty;

    public DateOnly IssuedAt { get; private set; }

    public DateOnly? ExpiresAt { get; private set; }

    public PassportType Type { get; private set; }

    public PassportStatus Status { get; private set; }

    protected Passport()
    {
    }

    public Passport(
        Guid id,
        Citizen citizen,
        PassportSeries series,
        PassportNumber number,
        DepartmentCode departmentCode,
        string issuedBy,
        DateOnly issuedAt,
        PassportType type = PassportType.Internal,
        DateOnly? expiresAt = null) : base(id)
    {
        Citizen = citizen ?? throw new ArgumentNullValueException(nameof(citizen));
        CitizenId = citizen.Id;
        Series = series ?? throw new ArgumentNullValueException(nameof(series));
        Number = number ?? throw new ArgumentNullValueException(nameof(number));
        DepartmentCode = departmentCode ?? throw new ArgumentNullValueException(nameof(departmentCode));
        IssuedBy = string.IsNullOrWhiteSpace(issuedBy) ? throw new ArgumentException("Орган выдачи обязателен.", nameof(issuedBy)) : issuedBy.Trim();
        IssuedAt = issuedAt;
        ExpiresAt = expiresAt;
        Type = type;
        Status = PassportStatus.Active;

        ValidatePeriod(issuedAt, expiresAt);
    }

    public string FullNumber => $"{Series.Value} {Number.Value}";

    public void MarkReplaced(DateOnly replacedAt)
    {
        EnsureFinalStatusNotSet();
        if (replacedAt < IssuedAt)
            throw new PassportValidityException("Дата замены паспорта не может быть раньше даты выдачи.");

        Status = PassportStatus.Replaced;
        ExpiresAt = replacedAt;
    }

    public void MarkLost()
    {
        EnsureFinalStatusNotSet();
        Status = PassportStatus.Lost;
    }

    public void Revoke(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new PassportValidityException("Для аннулирования паспорта нужно указать причину.");

        EnsureFinalStatusNotSet();
        Status = PassportStatus.Revoked;
    }

    public void MarkExpired(DateOnly date)
    {
        if (ExpiresAt is not null && date >= ExpiresAt.Value)
            Status = PassportStatus.Expired;
    }

    private void EnsureFinalStatusNotSet()
    {
        if (Status != PassportStatus.Active)
            throw new PassportValidityException("Операция доступна только для активного паспорта.");
    }

    private static void ValidatePeriod(DateOnly issuedAt, DateOnly? expiresAt)
    {
        if (issuedAt > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new PassportValidityException("Дата выдачи паспорта не может быть в будущем.");

        if (expiresAt is not null && expiresAt <= issuedAt)
            throw new PassportValidityException("Дата окончания действия паспорта должна быть позже даты выдачи.");
    }
}
