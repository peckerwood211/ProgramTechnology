using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Passports;

public record IssuePassportModel(
    Guid CitizenId,
    string Series,
    string Number,
    string DepartmentCode,
    string IssuedBy,
    DateOnly IssuedAt,
    PassportType Type = PassportType.Internal,
    DateOnly? ExpiresAt = null) : ICreateModel;

