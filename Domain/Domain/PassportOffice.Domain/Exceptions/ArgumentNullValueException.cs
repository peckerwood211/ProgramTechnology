namespace PassportOffice.Domain.Exceptions;

public sealed class ArgumentNullValueException(string parameterName)
    : ArgumentNullException(parameterName, "Значение доменной модели не может быть null.");

