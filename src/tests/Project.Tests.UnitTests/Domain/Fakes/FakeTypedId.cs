using Project.Domain.SeedWork;

namespace Project.Tests.UnitTests.Domain.Fakes
{
    public class FakeTypedId(Guid value) : TypedIdValueBase(value)
    {
    }
}