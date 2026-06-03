namespace PassportOffice.Domain.Exceptions;

public sealed class ApplicationStateException(string message) : DomainException(message);

