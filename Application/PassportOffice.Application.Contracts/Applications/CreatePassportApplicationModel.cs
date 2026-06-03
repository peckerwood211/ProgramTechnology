using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Applications;

public record CreatePassportApplicationModel(
    Guid CitizenId,
    string DepartmentCode,
    string ServiceCode,
    ApplicationType Type,
    string? Comment = null,
    IReadOnlyCollection<CreateAttachedDocumentModel>? Documents = null) : ICreateModel;

