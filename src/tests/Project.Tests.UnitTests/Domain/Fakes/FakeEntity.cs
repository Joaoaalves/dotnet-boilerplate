using Project.Domain.SeedWork;

namespace Project.Tests.UnitTests.Domain.Fakes
{
    public class FakeEntity : Entity
    {
        public void AddEvent(IDomainEvent domainEvent) => AddDomainEvent(domainEvent);
        public void ValidateRule(IBusinessRule rule) => CheckRule(rule);
    }
}