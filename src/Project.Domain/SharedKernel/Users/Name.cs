using Project.Domain.SeedWork;

namespace Project.Domain.SharedKernel.Users
{
    /// <summary>
    /// Represents a domain-specific name value object.
    /// </summary>
    public class Name : ValueObject
    {
        /// <summary>
        /// Gets the underlying string value of the name.
        /// </summary>
        public string Value { get; private set; } = string.Empty;

        /// <summary>
        /// Required by Entity Framework. Do not use directly.
        /// </summary>
        protected Name() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Name"/> class.
        /// </summary>
        /// <param name="value">The name string.</param>
        /// <exception cref="BusinessRuleValidationException">
        /// Thrown when the name does not satisfy the <see cref="Rules.NameMustBeValidRule"/>.
        /// </exception>
        public Name(string value)
        {
            SetValue(value);
        }

        /// <summary>
        /// Updates the name value.
        /// </summary>
        /// <param name="value">The new name value.</param>
        /// <exception cref="BusinessRuleValidationException">
        /// Thrown when the new name does not satisfy the <see cref="Rules.NameMustBeValidRule"/>.
        /// </exception>
        public void SetValue(string value)
        {
            value = value.Trim();
            CheckRule(new Rules.NameMustBeValidRule(value));
            Value = value;
        }

        /// <inheritdoc />
        public override string ToString() => Value;

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is Name otherName)
            {
                return Value.Equals(otherName.Value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var lower = Value?.ToLowerInvariant();
            return lower?.GetHashCode() ?? 0;
        }
    }
}
