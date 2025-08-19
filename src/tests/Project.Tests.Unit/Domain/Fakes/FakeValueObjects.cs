using Project.Domain.SeedWork;

namespace Project.Tests.Unit.Domain.Fakes
{
    public class SimpleValueObject(int id) : ValueObject
    {
        public int Id { get; } = id;
    }

    public class FieldValueObject(string name) : ValueObject
    {
        private readonly string _name = name;
    }

    public class MixedValueObject(int age, string name) : ValueObject
    {
        private readonly int _age = age;
        public string Name { get; } = name;
    }

    public class IgnoredValueObject : ValueObject
    {
        [IgnoreMember]
        public int IgnoredProp => 42;
#pragma warning disable CS0414
        [IgnoreMember]
        private readonly string _ignoredField = "secret";
#pragma warning disable CS0414
        public string VisibleProp => "visible";
    }
}