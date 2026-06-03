using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Validators;

namespace PassportOffice.ValueObjects;

public sealed class ServiceCode : ValueObject<string>
{
    private static readonly ServiceCodeValidator Validator = new();

    private ServiceCode()
    {
    }

    public ServiceCode(string value) : base(Validator, value?.Trim().ToUpperInvariant() ?? string.Empty)
    {
    }
}

