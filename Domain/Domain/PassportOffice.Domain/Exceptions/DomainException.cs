namespace PassportOffice.Domain.Exceptions;

public abstract class DomainException(string message) : InvalidOperationException(message);

