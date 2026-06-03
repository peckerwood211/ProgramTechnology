using System.Text.RegularExpressions;
using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.ValueObjects.Validators;

public sealed class PhoneNumberValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectValidationException(nameof(PhoneNumber), "телефон обязателен");

        if (!Regex.IsMatch(value.Trim(), @"^\+?[0-9 ()-]{7,20}$"))
            throw new ValueObjectValidationException(nameof(PhoneNumber), "телефон имеет неверный формат");
    }
}

