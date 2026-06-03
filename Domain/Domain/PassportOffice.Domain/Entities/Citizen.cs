using PassportOffice.Domain.Base;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;
using PassportOffice.ValueObjects;

namespace PassportOffice.Domain.Entities;

public class Citizen : Entity<Guid>, IAggregateRoot
{
    private readonly List<Passport> _passports = new();
    private readonly List<AddressRegistration> _registrations = new();
    private readonly List<PassportApplication> _applications = new();
    private readonly List<Appointment> _appointments = new();

    public FullName FullName { get; private set; } = default!;

    public DateOnly BirthDate { get; private set; }

    public string BirthPlace { get; private set; } = string.Empty;

    public Gender Gender { get; private set; }

    public string Snils { get; private set; } = string.Empty;

    public PhoneNumber? Phone { get; private set; }

    public EmailAddress? Email { get; private set; }

    public IReadOnlyCollection<Passport> Passports => _passports.AsReadOnly();

    public IReadOnlyCollection<AddressRegistration> Registrations => _registrations.AsReadOnly();

    public IReadOnlyCollection<PassportApplication> Applications => _applications.AsReadOnly();

    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    protected Citizen()
    {
    }

    public Citizen(
        Guid id,
        FullName fullName,
        DateOnly birthDate,
        string birthPlace,
        Gender gender,
        string snils,
        PhoneNumber? phone = null,
        EmailAddress? email = null) : base(id)
    {
        FullName = fullName ?? throw new ArgumentNullValueException(nameof(fullName));
        BirthDate = birthDate;
        BirthPlace = string.IsNullOrWhiteSpace(birthPlace)
            ? throw new ArgumentException("Место рождения обязательно.", nameof(birthPlace))
            : birthPlace.Trim();
        Gender = gender;
        Snils = snils?.Trim() ?? string.Empty;
        Phone = phone;
        Email = email;

        ValidateBirthDate(birthDate);
    }

    public Passport? CurrentPassport
        => _passports
            .Where(passport => passport.Status == PassportStatus.Active)
            .OrderByDescending(passport => passport.IssuedAt)
            .FirstOrDefault();

    public AddressRegistration? CurrentPermanentRegistration
        => _registrations
            .Where(registration => registration.Type == RegistrationType.Permanent && registration.IsActiveOn(DateOnly.FromDateTime(DateTime.UtcNow)))
            .OrderByDescending(registration => registration.ValidFrom)
            .FirstOrDefault();

    public void ChangeFullName(FullName fullName)
        => FullName = fullName ?? throw new ArgumentNullValueException(nameof(fullName));

    public void UpdateContacts(PhoneNumber? phone, EmailAddress? email)
    {
        Phone = phone;
        Email = email;
    }

    public Passport IssuePassport(
        PassportSeries series,
        PassportNumber number,
        DepartmentCode departmentCode,
        string issuedBy,
        DateOnly issuedAt,
        PassportType type = PassportType.Internal,
        DateOnly? expiresAt = null)
    {
        if (_passports.Any(passport => passport.Series == series && passport.Number == number))
            throw new PassportValidityException("Паспорт с такой серией и номером уже есть у гражданина.");

        var activePassport = CurrentPassport;
        activePassport?.MarkReplaced(issuedAt);

        var passport = new Passport(
            Guid.NewGuid(),
            this,
            series,
            number,
            departmentCode,
            issuedBy,
            issuedAt,
            type,
            expiresAt);

        _passports.Add(passport);
        return passport;
    }

    public AddressRegistration RegisterAddress(
        AddressLine address,
        RegistrationType type,
        DateOnly registeredAt,
        DateOnly validFrom,
        DateOnly? validTo = null,
        string? comment = null)
    {
        if (type == RegistrationType.Permanent)
        {
            foreach (var registration in _registrations.Where(registration =>
                         registration.Type == RegistrationType.Permanent
                         && registration.Status == RegistrationStatus.Active))
            {
                registration.Close(validFrom, "Закрыта новой постоянной регистрацией.");
            }
        }

        var newRegistration = new AddressRegistration(
            Guid.NewGuid(),
            this,
            address,
            type,
            registeredAt,
            validFrom,
            validTo,
            comment);

        _registrations.Add(newRegistration);
        return newRegistration;
    }

    public PassportApplication SubmitApplication(
        ServiceCatalogItem service,
        OfficeDepartment department,
        ApplicationType type,
        DateTime submittedAt,
        string? comment = null)
    {
        if (service.ApplicationType != type)
            throw new ApplicationStateException("Тип услуги не совпадает с типом заявления.");

        var application = new PassportApplication(
            Guid.NewGuid(),
            this,
            service,
            department,
            type,
            submittedAt,
            comment);

        _applications.Add(application);
        return application;
    }

    public Appointment BookAppointment(
        OfficeDepartment department,
        DateTime scheduledAt,
        string purpose,
        OfficeEmployee? employee = null,
        PassportApplication? application = null)
    {
        var appointment = new Appointment(
            Guid.NewGuid(),
            this,
            department,
            scheduledAt,
            purpose,
            employee,
            application);

        _appointments.Add(appointment);
        return appointment;
    }

    private static void ValidateBirthDate(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (birthDate > today)
            throw new InvalidBirthDateException(birthDate);
    }
}
