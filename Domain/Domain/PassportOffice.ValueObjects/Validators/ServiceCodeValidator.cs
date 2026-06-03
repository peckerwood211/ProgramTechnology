using System.Text.RegularExpressions;
using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.ValueObjects.Validators;

public sealed class ServiceCodeValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (!Regex.IsMatch(value ?? string.Empty, @"^[A-Z0-9_-]{3,20}$"))
            throw new ValueObjectValidationException(nameof(ServiceCode), "код услуги должен содержать 3-20 латинских символов, цифр, '_' или '-'");
    }
}

