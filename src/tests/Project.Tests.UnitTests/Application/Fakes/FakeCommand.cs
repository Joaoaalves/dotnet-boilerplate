using Project.Application.Configuration.Commands;
using Project.Domain.SeedWork;

namespace Project.Tests.UnitTests.Application.Fakes
{
    public class FakeCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class FakeCommandString : ICommand<string>
    {
        public Guid Id => Guid.NewGuid();
    }
}