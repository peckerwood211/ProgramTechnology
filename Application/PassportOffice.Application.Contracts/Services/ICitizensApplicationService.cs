using PassportOffice.Application.Contracts.Citizens;
using PassportOffice.Application.Contracts.Passports;

namespace PassportOffice.Application.Contracts.Services;

public interface ICitizensApplicationService
    : IApplicationService<CitizenModel, CreateCitizenModel, Guid>
{
    Task<IReadOnlyCollection<CitizenModel>> SearchAsync(string query, CancellationToken cancellationToken = default);

    Task<bool> UpdateContactsAsync(Guid id, UpdateCitizenContactsModel model, CancellationToken cancellationToken = default);

    Task<bool> ChangeNameAsync(Guid id, ChangeCitizenNameModel model, CancellationToken cancellationToken = default);

    Task<PassportModel?> IssuePassportAsync(IssuePassportModel model, CancellationToken cancellationToken = default);

    Task<AddressRegistrationModel?> RegisterAddressAsync(RegisterAddressModel model, CancellationToken cancellationToken = default);
}

