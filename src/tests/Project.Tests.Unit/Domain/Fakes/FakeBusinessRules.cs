using Project.Domain.SeedWork;

namespace Project.Tests.Unit.Domain.Fakes
{
    public class FakeValidRule : IBusinessRule
    {
        public bool IsBroken() => false;
        public string Message => "Should not be thrown";
    }

    public class FakeBrokenRule : IBusinessRule
    {
        public bool IsBroken() => true;
        public string Message => "Rule was broken";
    }
}