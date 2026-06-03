using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Validators;

namespace PassportOffice.ValueObjects;

public sealed class EmailAddress : ValueObject<string>
{
    private static readonly EmailAddressValidator Validator = new();

    private EmailAddress()
    {
    }

    public EmailAddress(string value) : base(Validator, value?.Trim().ToLowerInvariant() ?? string.Empty)
    {
    }
}

