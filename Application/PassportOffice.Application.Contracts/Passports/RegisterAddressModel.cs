using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Passports;

public record RegisterAddressModel(
    Guid CitizenId,
    string Address,
    RegistrationType Type,
    DateOnly RegisteredAt,
    DateOnly ValidFrom,
    DateOnly? ValidTo = null,
    string? Comment = null) : ICreateModel;

