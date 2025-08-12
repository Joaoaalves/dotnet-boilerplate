using Project.Domain.SeedWork;

namespace Project.Domain.Users
{
    /// <summary>
    /// Represents a strongly-typed identifier for the User aggregate.
    /// </summary>
    public sealed class UserId : TypedIdValueBase
    {
        public UserId(Guid id) : base(id) { }
        public UserId(string id) : base(Guid.Parse(id)) { }

        /// <summary>
        /// Creates a new unique <see cref="UserId"/>.
        /// </summary>
        public static UserId NewId() => new(Guid.NewGuid());

        /// <summary>
        /// Converts the TypedId to String
        /// </summary>
    }
}