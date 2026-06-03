using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.ValueObjects.Validators;

public sealed class FullNameValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectValidationException(nameof(FullName), "ФИО обязательно");

        var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            throw new ValueObjectValidationException(nameof(FullName), "укажите минимум фамилию и имя");

        if (value.Length > 120)
            throw new ValueObjectValidationException(nameof(FullName), "ФИО не должно превышать 120 символов");
    }
}

