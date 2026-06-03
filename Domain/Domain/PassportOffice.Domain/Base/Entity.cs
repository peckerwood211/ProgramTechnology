namespace PassportOffice.Domain.Base;

public abstract class Entity<TId> where TId : struct, IEquatable<TId>
{
    public TId Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
        => obj is Entity<TId> entity && entity.Id.Equals(Id);

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);
}

