namespace PassportOffice.ValueObjects.Exceptions;

public class ValueObjectValidationException(string valueObjectName, string message)
    : ArgumentException($"{valueObjectName}: {message}");

