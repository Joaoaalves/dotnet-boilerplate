namespace Project.Domain.SeedWork
{
    /// <summary>
    /// Represents a domain event that indicates something important has happened within the domain.
    /// Domain events are used to notify other parts of the system about changes in state.
    /// </summary>
    public interface IDomainEvent : INotification
    {
        /// <summary>
        /// Gets the timestamp indicating when the domain event occurred.
        /// </summary>
        DateTime OccurredOn { get; }
    }
}
