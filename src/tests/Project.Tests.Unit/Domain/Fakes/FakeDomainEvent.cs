using Project.Domain.SeedWork;

namespace Project.Tests.Unit.Domain.Fakes
{
    public class FakeDomainEvent : IDomainEvent
    {
        public DateTime OccurredOn => new();
    }
}