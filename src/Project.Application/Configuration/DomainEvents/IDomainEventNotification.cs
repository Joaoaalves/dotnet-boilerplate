using Project.Domain.SeedWork;

namespace Project.Application.Configuration.DomainEvents
{
    public interface IDomainEventNotification<TEventType> : IDomainEventNotification
    {
        TEventType DomainEvent { get; }
    }

    public interface IDomainEventNotification : INotification
    {
        Guid Id { get; }
    }
}