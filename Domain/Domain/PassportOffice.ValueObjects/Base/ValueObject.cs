namespace PassportOffice.ValueObjects.Base;

public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
{
    public T Value { get; protected set; } = default!;

    protected ValueObject()
    {
    }

    protected ValueObject(IValidator<T> validator, T value)
    {
        ArgumentNullException.ThrowIfNull(validator);
        validator.Validate(value);
        Value = value;
    }

    public override string ToString()
        => Value?.ToString() ?? GetType().Name;

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Value);

    public override bool Equals(object? obj)
        => Equals(obj as ValueObject<T>);

    public bool Equals(ValueObject<T>? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return GetType() == other.GetType()
            && EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
        => Equals(left, right);

    public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
        => !(left == right);
}

