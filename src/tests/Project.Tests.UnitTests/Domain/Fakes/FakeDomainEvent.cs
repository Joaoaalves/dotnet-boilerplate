using Project.Domain.SeedWork;

namespace Project.Tests.UnitTests.Domain.Fakes
{
    public class FakeDomainEvent : IDomainEvent
    {
        public DateTime OccurredOn => new();
    }
}