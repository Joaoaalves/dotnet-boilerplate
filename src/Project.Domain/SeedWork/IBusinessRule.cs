namespace Project.Domain.SeedWork
{
    /// <summary>
    /// Represents a business rule that can be validated.
    /// Business rules encapsulate domain-specific logic that must be enforced.
    /// </summary>
    public interface IBusinessRule
    {
        /// <summary>
        /// Determines whether the rule is broken.
        /// </summary>
        /// <returns><c>true</c> if the rule is broken; otherwise, <c>false</c>.</returns>
        bool IsBroken();

        /// <summary>
        /// Gets the validation message describing why the rule is broken.
        /// </summary>
        string Message { get; }
    }
}
