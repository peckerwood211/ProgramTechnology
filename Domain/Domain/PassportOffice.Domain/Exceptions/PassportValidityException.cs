namespace PassportOffice.Domain.Exceptions;

public sealed class PassportValidityException(string message) : DomainException(message);

