using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Passports;

public record PassportModel(
    Guid Id,
    Guid CitizenId,
    string Series,
    string Number,
    string FullNumber,
    string DepartmentCode,
    string IssuedBy,
    DateOnly IssuedAt,
    DateOnly? ExpiresAt,
    PassportType Type,
    PassportStatus Status) : IModel<Guid>;

