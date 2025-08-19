using Project.Domain.SeedWork;

namespace Project.Tests.Unit.Domain.Fakes
{
    public class FakeTypedId(Guid value) : TypedIdValueBase(value)
    {
    }
}