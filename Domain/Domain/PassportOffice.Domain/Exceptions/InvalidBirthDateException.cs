namespace PassportOffice.Domain.Exceptions;

public sealed class InvalidBirthDateException(DateOnly birthDate)
    : DomainException($"Дата рождения {birthDate:dd.MM.yyyy} не может быть в будущем.");
