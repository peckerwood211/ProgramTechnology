using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Validators;

namespace PassportOffice.ValueObjects;

public sealed class PassportNumber : ValueObject<string>
{
    private static readonly PassportNumberValidator Validator = new();

    private PassportNumber()
    {
    }

    public PassportNumber(string value) : base(Validator, value?.Trim() ?? string.Empty)
    {
    }
}

