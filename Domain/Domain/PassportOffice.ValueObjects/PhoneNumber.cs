using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Validators;

namespace PassportOffice.ValueObjects;

public sealed class PhoneNumber : ValueObject<string>
{
    private static readonly PhoneNumberValidator Validator = new();

    private PhoneNumber()
    {
    }

    public PhoneNumber(string value) : base(Validator, value?.Trim() ?? string.Empty)
    {
    }
}

