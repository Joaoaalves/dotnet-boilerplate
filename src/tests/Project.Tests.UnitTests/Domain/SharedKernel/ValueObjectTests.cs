using Project.Tests.UnitTests.Domain.Fakes;

namespace Project.Tests.UnitTests.Domain.SharedKernel
{
    public class ValueObjectTests
    {
        [Fact]
        public void Equals_ShouldReturnTrue_ForSamePropertyValue()
        {
            var vo1 = new SimpleValueObject(1);
            var vo2 = new SimpleValueObject(1);

            Assert.True(vo1.Equals(vo2));
            Assert.True(vo1 == vo2);
            Assert.False(vo1 != vo2);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentPropertyValue()
        {
            var vo1 = new SimpleValueObject(1);
            var vo2 = new SimpleValueObject(2);

            Assert.False(vo1.Equals(vo2));
            Assert.False(vo1 == vo2);
            Assert.True(vo1 != vo2);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingWithNull()
        {
            var vo = new SimpleValueObject(1);
            SimpleValueObject? vo2 = null;

            Assert.False(vo.Equals(vo2));
            Assert.False(vo == null);
            Assert.True(vo != null);
        }


        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingDifferentTypesWithSecondBeingNull()
        {
            var vo1 = new SimpleValueObject(1);
            FieldValueObject? vo2 = null;

            Assert.False(vo1.Equals(vo2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparingDifferentTypes()
        {
            var vo1 = new SimpleValueObject(1);
            var vo2 = new FieldValueObject("1");

            Assert.False(vo1.Equals(vo2));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameFieldValue()
        {
            var vo1 = new FieldValueObject("test");
            var vo2 = new FieldValueObject("test");

            Assert.True(vo1.Equals(vo2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentFieldValue()
        {
            var vo1 = new FieldValueObject("a");
            var vo2 = new FieldValueObject("b");

            Assert.False(vo1.Equals(vo2));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenAllFieldsAndPropertiesAreEqual()
        {
            var vo1 = new MixedValueObject(10, "Alice");
            var vo2 = new MixedValueObject(10, "Alice");

            Assert.True(vo1.Equals(vo2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenAnyFieldOrPropertyDiffers()
        {
            var vo1 = new MixedValueObject(10, "Alice");
            var vo2 = new MixedValueObject(20, "Alice");

            Assert.False(vo1.Equals(vo2));
        }

        [Fact]
        public void EqualityOperator_ShouldReturnTrue_WhenBothAreNull()
        {
            SimpleValueObject? obj1 = null;
            SimpleValueObject? obj2 = null;

            var result = obj1 == obj2;

            Assert.True(result);
        }
        [Fact]
        public void GetHashCode_ShouldBeEqual_ForEqualObjects()
        {
            var vo1 = new MixedValueObject(10, "Bob");
            var vo2 = new MixedValueObject(10, "Bob");

            Assert.Equal(vo1.GetHashCode(), vo2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ShouldDiffer_ForDifferentObjects()
        {
            var vo1 = new MixedValueObject(10, "Bob");
            var vo2 = new MixedValueObject(10, "Alice");

            Assert.NotEqual(vo1.GetHashCode(), vo2.GetHashCode());
        }

        [Fact]
        public void IgnoredMembers_ShouldNotAffectEqualityOrHashCode()
        {
            var vo1 = new IgnoredValueObject();
            var vo2 = new IgnoredValueObject();

            Assert.True(vo1.Equals(vo2));
            Assert.Equal(vo1.GetHashCode(), vo2.GetHashCode());
        }


    }
}