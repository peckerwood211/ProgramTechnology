using PassportOffice.Domain.Base;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;

namespace PassportOffice.Domain.Entities;

public class AttachedDocument : Entity<Guid>
{
    public Guid ApplicationId { get; private set; }

    public PassportApplication Application { get; private set; } = default!;

    public AttachedDocumentType Type { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Number { get; private set; } = string.Empty;

    public DateOnly ReceivedAt { get; private set; }

    protected AttachedDocument()
    {
    }

    public AttachedDocument(
        Guid id,
        PassportApplication application,
        AttachedDocumentType type,
        string name,
        string? number,
        DateOnly receivedAt) : base(id)
    {
        Application = application ?? throw new ArgumentNullValueException(nameof(application));
        ApplicationId = application.Id;
        Type = type;
        Name = string.IsNullOrWhiteSpace(name) ? type.ToString() : name.Trim();
        Number = number?.Trim() ?? string.Empty;
        ReceivedAt = receivedAt;
    }
}

