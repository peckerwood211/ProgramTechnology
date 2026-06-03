using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Passports;

public record AddressRegistrationModel(
    Guid Id,
    Guid CitizenId,
    string Address,
    RegistrationType Type,
    RegistrationStatus Status,
    DateOnly RegisteredAt,
    DateOnly ValidFrom,
    DateOnly? ValidTo,
    string Comment) : IModel<Guid>;

