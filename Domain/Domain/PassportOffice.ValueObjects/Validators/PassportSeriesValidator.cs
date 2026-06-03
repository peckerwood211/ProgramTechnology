using System.Text.RegularExpressions;
using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.ValueObjects.Validators;

public sealed class PassportSeriesValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (!Regex.IsMatch(value ?? string.Empty, @"^\d{4}$"))
            throw new ValueObjectValidationException(nameof(PassportSeries), "серия паспорта должна состоять из 4 цифр");
    }
}

