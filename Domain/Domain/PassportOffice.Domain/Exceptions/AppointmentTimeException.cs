namespace PassportOffice.Domain.Exceptions;

public sealed class AppointmentTimeException(string message) : DomainException(message);

