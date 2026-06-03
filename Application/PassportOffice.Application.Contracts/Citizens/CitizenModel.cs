using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Citizens;

public record CitizenModel(
    Guid Id,
    string FullName,
    DateOnly BirthDate,
    string BirthPlace,
    Gender Gender,
    string Snils,
    string? Phone,
    string? Email,
    string? CurrentPassport,
    string? CurrentRegistration,
    int ApplicationsCount) : IModel<Guid>;

