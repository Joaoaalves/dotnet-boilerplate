using Project.Domain.SeedWork;

namespace Project.Tests.Unit.Domain.Fakes
{
    public class FakeEntity : Entity
    {
        public void AddEvent(IDomainEvent domainEvent) => AddDomainEvent(domainEvent);
        public void ValidateRule(IBusinessRule rule) => CheckRule(rule);
    }
}