namespace Project.Infrastructure.Processing
{
    /// <summary>
    /// Defines a service responsible for dispatching domain events.
    /// </summary>
    public interface IDomainEventsDispatcher
    {
        /// <summary>
        /// Dispatches all domain events that have been collected from tracked entities.
        /// </summary>
        Task DispatchEventsAsync();
    }
}
