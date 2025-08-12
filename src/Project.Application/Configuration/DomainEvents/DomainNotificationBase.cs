using System.Text.Json.Serialization;
using Project.Domain.SeedWork;

namespace Project.Application.Configuration.DomainEvents
{
    public class DomainNotificationBase<T>(T domainEvent) : IDomainEventNotification<T> where T : IDomainEvent
    {
        [JsonIgnore]
        public T DomainEvent { get; } = domainEvent;

        public Guid Id { get; } = Guid.NewGuid();
    }
}