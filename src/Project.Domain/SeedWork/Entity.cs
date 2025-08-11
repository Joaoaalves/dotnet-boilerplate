namespace Project.Domain.SeedWork
{
    /// <summary>
    /// Represents an abstract base class for all entities.
    /// </summary>
    public abstract class Entity2
    {
        private List<IDomainEvent>? _domainEvents;

        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= [];
            _domainEvents.Add(domainEvent);
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}