using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.ValueObjects.Validators;

public sealed class AddressLineValidator : IValidator<string>
{
    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValueObjectValidationException(nameof(AddressLine), "адрес обязателен");

        if (value.Trim().Length < 10)
            throw new ValueObjectValidationException(nameof(AddressLine), "адрес должен содержать минимум 10 символ");

        if (value.Trim().Length > 250)
            throw new ValueObjectValidationException(nameof(AddressLine), "адрес не должен превышать 250 символов");
    }
}

