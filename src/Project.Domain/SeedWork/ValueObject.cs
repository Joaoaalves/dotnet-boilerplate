using System.Reflection;

namespace Project.Domain.SeedWork
{
    /// <summary>
    /// Represents a base class for value objects.
    /// Value objects are immutable and compared by the values of their properties and fields.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        private List<PropertyInfo> _properties = [];
        private List<FieldInfo> _fields = [];

        /// <summary>
        /// Equality operator for value objects.
        /// </summary>
        public static bool operator ==(ValueObject? obj1, ValueObject? obj2)
        {
            if (Equals(obj1, null))
            {
                return Equals(obj2, null);
            }

            return obj1.Equals(obj2);
        }

        /// <summary>
        /// Inequality operator for value objects.
        /// </summary>
        public static bool operator !=(ValueObject? obj1, ValueObject? obj2)
        {
            return !(obj1 == obj2);
        }

        /// <inheritdoc />
        public bool Equals(ValueObject? obj)
        {
            if (obj is null)
                return false;

            return Equals((object)obj);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            return Equals(p.GetValue(this, null), p.GetValue(obj, null));
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            return Equals(f.GetValue(this), f.GetValue(obj));
        }

        private List<PropertyInfo> GetProperties()
        {
            _properties ??= GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<IgnoreMemberAttribute>() == null)
                .ToList();

            return _properties;
        }

        private List<FieldInfo> GetFields()
        {
            _fields ??= GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.GetCustomAttribute<IgnoreMemberAttribute>() == null)
                .ToList();

            return _fields;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                foreach (var prop in GetProperties())
                {
                    var value = prop.GetValue(this, null);
                    hash = HashValue(hash, value);
                }

                foreach (var field in GetFields())
                {
                    var value = field.GetValue(this);
                    hash = HashValue(hash, value);
                }

                return hash;
            }
        }

        private static int HashValue(int seed, object? value)
        {
            var currentHash = value?.GetHashCode() ?? 0;
            return seed * 23 + currentHash;
        }

        /// <summary>
        /// Validates a business rule and throws a <see cref="BusinessRuleValidationException"/> if the rule is broken.
        /// </summary>
        /// <param name="rule">The business rule to validate.</param>
        /// <exception cref="BusinessRuleValidationException">Thrown if the rule is broken.</exception>
        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}
