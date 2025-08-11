namespace Project.Domain.SeedWork
{
    /// <summary>
    /// Base class for strongly-typed identifier value objects.
    /// Ensures consistent equality and hash code logic for identifiers.
    /// </summary>
    public abstract class TypedIdValueBase(Guid value) : IEquatable<TypedIdValueBase>
    {
        /// <summary>
        /// Gets the underlying GUID value of the identifier.
        /// </summary>
        public Guid Value { get; } = value;

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            return obj is TypedIdValueBase other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc />
        public bool Equals(TypedIdValueBase? other)
        {
            if (other is null)
                return false;

            return Value == other.Value;
        }

        /// <summary>
        /// Checks equality between two TypedIdValueBase instances.
        /// </summary>
        public static bool operator ==(TypedIdValueBase obj1, TypedIdValueBase obj2)
        {
            if (object.Equals(obj1, null))
            {
                return object.Equals(obj2, null);
            }

            return obj1.Equals(obj2);
        }

        /// <summary>
        /// Checks inequality between two TypedIdValueBase instances.
        /// </summary>
        public static bool operator !=(TypedIdValueBase x, TypedIdValueBase y)
        {
            return !(x == y);
        }
    }
}
