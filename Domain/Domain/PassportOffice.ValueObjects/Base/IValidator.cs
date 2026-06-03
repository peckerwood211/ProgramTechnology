namespace PassportOffice.ValueObjects.Base;

public interface IValidator<in T>
{
    void Validate(T value);
}

