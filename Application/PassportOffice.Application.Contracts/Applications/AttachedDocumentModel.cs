using PassportOffice.Application.Contracts.Base;
using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Applications;

public record AttachedDocumentModel(
    Guid Id,
    Guid ApplicationId,
    AttachedDocumentType Type,
    string Name,
    string Number,
    DateOnly ReceivedAt) : IModel<Guid>;

