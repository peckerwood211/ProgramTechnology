using System.Text.RegularExpressions;
using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.ValueObjects.Validators;

public sealed class EmailAddressValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectValidationException(nameof(EmailAddress), "email обязателен");

        if (!Regex.IsMatch(value.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ValueObjectValidationException(nameof(EmailAddress), "email имеет неверный формат");
    }
}

