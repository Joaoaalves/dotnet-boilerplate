namespace Project.Domain.SeedWork
{
    /// <summary>
    /// Represents the base class for all entities in the domain layer.
    /// Provides common behavior such as domain event handling and business rule validation.
    /// </summary>
    public abstract class Entity
    {
        private List<IDomainEvent>? _domainEvents;

        /// <summary>
        /// Gets a read-only collection of domain events that have been raised by this entity.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// Adds a domain event to the entity's list of events.
        /// This should be called whenever a significant change occurs in the entity that other parts of the system should react to.
        /// </summary>
        /// <param name="domainEvent">The domain event to add.</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= [];
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Checks a business rule and throws a <see cref="BusinessRuleValidationException"/> if the rule is broken.
        /// Use this to enforce domain invariants before performing operations.
        /// </summary>
        /// <param name="rule">The business rule to validate.</param>
        /// <exception cref="BusinessRuleValidationException">Thrown when the business rule is broken.</exception>
        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        /// <summary>
        /// Clears all domain events associated with this entity.
        /// This is typically called after the events have been dispatched.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
