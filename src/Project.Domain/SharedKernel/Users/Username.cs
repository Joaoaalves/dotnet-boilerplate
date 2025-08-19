using Project.Domain.SeedWork;

namespace Project.Domain.SharedKernel.Users
{
    /// <summary>
    /// Represents a domain-specific username value object.
    /// </summary>
    public class UserName : ValueObject
    {
        /// <summary>
        /// Gets the underlying string value of the username.
        /// </summary>
        public string Value { get; private set; } = string.Empty;

        /// <summary>
        /// Required by Entity Framework. Do not use directly.
        /// </summary>
        protected UserName() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserName"/> class.
        /// </summary>
        /// <param name="value">The username string.</param>
        /// <exception cref="BusinessRuleValidationException">
        /// Thrown when the username does not satisfy the <see cref="Rules.UserNameMustBeValidRule"/>.
        /// </exception>
        public UserName(string value)
        {
            var userName = value.Trim();
            CheckRule(new Rules.UserNameMustBeValidRule(userName));
            Value = userName;
        }

        /// <summary>
        /// Implicit conversion from <see cref="UserName"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="userName">The <see cref="UserName"/> instance.</param>
        public static implicit operator string(UserName userName) => userName?.Value ?? string.Empty;

        /// <inheritdoc />
        public override string ToString() => Value;

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is UserName otherUserName)
            {
                return Value.Equals(otherUserName.Value, StringComparison.OrdinalIgnoreCase);
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
