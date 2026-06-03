using System.Text.RegularExpressions;
using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.ValueObjects.Validators;

public sealed class PassportNumberValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (!Regex.IsMatch(value ?? string.Empty, @"^\d{6}$"))
            throw new ValueObjectValidationException(nameof(PassportNumber), "номер паспорта должен состоять из 6 цифр");
    }
}

