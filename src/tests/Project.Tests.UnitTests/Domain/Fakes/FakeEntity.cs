using Project.Domain.SeedWork;

namespace Project.Tests.UnitTests.Domain.Fakes
{
    public class FakeEntity : Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public FakeEntity()
        {
            Name = "";
        }

        public FakeEntity(string name)
        {
            Name = name;
        }

        public void AddEvent(IDomainEvent domainEvent) => AddDomainEvent(domainEvent);
        public void ValidateRule(IBusinessRule rule) => CheckRule(rule);
    }
}