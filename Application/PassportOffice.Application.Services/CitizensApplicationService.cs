using PassportOffice.Application.Contracts.Citizens;
using PassportOffice.Application.Contracts.Passports;
using PassportOffice.Application.Contracts.Services;
using PassportOffice.Application.Services.Mapping;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Repositories.Abstractions;
using PassportOffice.Domain.Repositories.Abstractions.Base;
using PassportOffice.ValueObjects;

namespace PassportOffice.Application.Services;

public class CitizensApplicationService(
    ICitizenRepository citizens,
    IPassportRepository passports,
    IRepository<AddressRegistration, Guid> registrations,
    IUnitOfWork unitOfWork) : ICitizensApplicationService
{
    public async Task<IReadOnlyCollection<CitizenModel>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await citizens.GetAllAsync(cancellationToken, true))
            .Select(citizen => citizen.ToModel())
            .ToArray();

    public async Task<CitizenModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => (await citizens.GetByIdAsync(id, cancellationToken))?.ToModel();

    public async Task<IReadOnlyCollection<CitizenModel>> SearchAsync(string query, CancellationToken cancellationToken = default)
        => (await citizens.SearchByNameAsync(query, cancellationToken))
            .Select(citizen => citizen.ToModel())
            .ToArray();

    public async Task<CitizenModel?> CreateAsync(CreateCitizenModel model, CancellationToken cancellationToken = default)
    {
        var citizen = new Citizen(
            Guid.NewGuid(),
            new FullName(model.FullName),
            model.BirthDate,
            model.BirthPlace,
            model.Gender,
            model.Snils,
            string.IsNullOrWhiteSpace(model.Phone) ? null : new PhoneNumber(model.Phone),
            string.IsNullOrWhiteSpace(model.Email) ? null : new EmailAddress(model.Email));

        await citizens.AddAsync(citizen, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return citizen.ToModel();
    }

    public async Task<bool> UpdateContactsAsync(Guid id, UpdateCitizenContactsModel model, CancellationToken cancellationToken = default)
    {
        var citizen = await citizens.GetByIdAsync(id, cancellationToken);
        if (citizen is null)
            return false;

        citizen.UpdateContacts(
            string.IsNullOrWhiteSpace(model.Phone) ? null : new PhoneNumber(model.Phone),
            string.IsNullOrWhiteSpace(model.Email) ? null : new EmailAddress(model.Email));

        await citizens.UpdateAsync(citizen, cancellationToken);
        return await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> ChangeNameAsync(Guid id, ChangeCitizenNameModel model, CancellationToken cancellationToken = default)
    {
        var citizen = await citizens.GetByIdAsync(id, cancellationToken);
        if (citizen is null)
            return false;

        citizen.ChangeFullName(new FullName(model.FullName));
        await citizens.UpdateAsync(citizen, cancellationToken);
        return await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<PassportModel?> IssuePassportAsync(IssuePassportModel model, CancellationToken cancellationToken = default)
    {
        if (await passports.GetBySeriesAndNumberAsync(model.Series, model.Number, cancellationToken) is not null)
            return null;

        var citizen = await citizens.GetByIdAsync(model.CitizenId, cancellationToken);
        if (citizen is null)
            return null;

        var passport = citizen.IssuePassport(
            new PassportSeries(model.Series),
            new PassportNumber(model.Number),
            new DepartmentCode(model.DepartmentCode),
            model.IssuedBy,
            model.IssuedAt,
            model.Type,
            model.ExpiresAt);

        await passports.AddAsync(passport, cancellationToken);
        await citizens.UpdateAsync(citizen, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return passport.ToModel();
    }

    public async Task<AddressRegistrationModel?> RegisterAddressAsync(RegisterAddressModel model, CancellationToken cancellationToken = default)
    {
        var citizen = await citizens.GetByIdAsync(model.CitizenId, cancellationToken);
        if (citizen is null)
            return null;

        var registration = citizen.RegisterAddress(
            new AddressLine(model.Address),
            model.Type,
            model.RegisteredAt,
            model.ValidFrom,
            model.ValidTo,
            model.Comment);

        await registrations.AddAsync(registration, cancellationToken);
        await citizens.UpdateAsync(citizen, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return registration.ToModel();
    }
}

