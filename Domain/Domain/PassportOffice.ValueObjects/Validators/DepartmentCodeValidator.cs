using System.Text.RegularExpressions;
using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.ValueObjects.Validators;

public sealed class DepartmentCodeValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (!Regex.IsMatch(value ?? string.Empty, @"^\d{3}-\d{3}$"))
            throw new ValueObjectValidationException(nameof(DepartmentCode), "код подразделения должен иметь формат 000-000");
    }
}

