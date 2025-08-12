using Project.Domain.SeedWork;

namespace Project.Domain.SharedKernel.Users
{
    /// <summary>
    /// Represents a domain-specific email address value object.
    /// </summary>
    public class Email : ValueObject
    {
        /// <summary>
        /// Gets the underlying string value of the email address.
        /// </summary>
        public string Value { get; private set; } = string.Empty;

        /// <summary>
        /// Required by Entity Framework. Do not use directly.
        /// </summary>
        protected Email() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        /// <param name="value">The email address string.</param>
        /// <exception cref="BusinessRuleValidationException">
        /// Thrown when the email does not satisfy the <see cref="Rules.EmailMustBeValidRule"/>.
        /// </exception>
        public Email(string value)
        {
            CheckRule(new Rules.EmailMustBeValidRule(value));
            Value = value;
        }

        /// <summary>
        /// Implicit conversion from <see cref="Email"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="email">The <see cref="Email"/> instance.</param>
        public static implicit operator string(Email email)
        {
            return email.Value;
        }

        /// <inheritdoc />
        public override string ToString() => Value;

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is Email otherEmail)
            {
                return Value.Equals(otherEmail.Value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}
