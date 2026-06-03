using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Citizens;

public record CreateCitizenModel(
    string FullName,
    DateOnly BirthDate,
    string BirthPlace,
    Gender Gender,
    string Snils,
    string? Phone = null,
    string? Email = null) : ICreateModel;

