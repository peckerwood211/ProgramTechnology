using PassportOffice.Domain.Enums;

namespace PassportOffice.Application.Contracts.Applications;

public record CreateAttachedDocumentModel(
    AttachedDocumentType Type,
    string Name,
    string? Number,
    DateOnly ReceivedAt);

