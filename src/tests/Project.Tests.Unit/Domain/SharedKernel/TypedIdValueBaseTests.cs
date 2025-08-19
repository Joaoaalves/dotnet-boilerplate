using Project.Tests.Unit.Domain.Fakes;

namespace Project.Tests.Unit.Domain.SharedKernel
{
    public class TypedIdValueBaseTests
    {
        [Fact]
        public void Value_ShouldReturnProvidedGuid()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var id = new FakeTypedId(guid);

            // Assert
            Assert.Equal(guid, id.Value);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameValue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var id1 = new FakeTypedId(guid);
            var id2 = new FakeTypedId(guid);

            // Act
            var result = id1.Equals(id2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentValue()
        {
            // Arrange
            var id1 = new FakeTypedId(Guid.NewGuid());
            var id2 = new FakeTypedId(Guid.NewGuid());

            // Act
            var result = id1.Equals(id2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenOtherIsNull()
        {
            // Arrange
            var id = new FakeTypedId(Guid.NewGuid());

            // Act
            var result = id.Equals(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ObjectOverride_ShouldReturnTrue_ForSameValue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            object id1 = new FakeTypedId(guid);
            object id2 = new FakeTypedId(guid);

            // Act
            var result = id1.Equals(id2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ObjectOverride_ShouldReturnFalse_WhenOtherIsNull()
        {
            // Arrange
            var id = new FakeTypedId(Guid.NewGuid());

            // Act
            var result = id.Equals((object?)null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ObjectOverride_ShouldReturnFalse_WhenOtherIsDifferentType()
        {
            // Arrange
            var id = new FakeTypedId(Guid.NewGuid());
            var other = "some string";

            // Act
            var result = id.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void OperatorEqual_ShouldReturnTrue_ForSameValue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var id1 = new FakeTypedId(guid);
            var id2 = new FakeTypedId(guid);

            // Act
            var result = id1 == id2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OperatorEqual_ShouldReturnFalse_ForDifferentValues()
        {
            // Arrange
            var id1 = new FakeTypedId(Guid.NewGuid());
            var id2 = new FakeTypedId(Guid.NewGuid());

            // Act
            var result = id1 == id2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void OperatorEqual_ShouldReturnTrue_WhenBothAreNull()
        {
            // Arrange
            FakeTypedId? id1 = null;
            FakeTypedId? id2 = null;

            // Act
            var result = id1! == id2!;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OperatorEqual_ShouldReturnFalse_WhenOnlyOneIsNull()
        {
            // Arrange
            var id1 = new FakeTypedId(Guid.NewGuid());
            FakeTypedId? id2 = null;

            // Act
            var result1 = id1 == id2!;
            var result2 = id2! == id1;

            // Assert
            Assert.False(result1);
            Assert.False(result2);
        }

        [Fact]
        public void OperatorNotEqual_ShouldReturnTrue_ForDifferentValues()
        {
            // Arrange
            var id1 = new FakeTypedId(Guid.NewGuid());
            var id2 = new FakeTypedId(Guid.NewGuid());

            // Act
            var result = id1 != id2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OperatorNotEqual_ShouldReturnFalse_ForSameValues()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var id1 = new FakeTypedId(guid);
            var id2 = new FakeTypedId(guid);

            // Act
            var result = id1 != id2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameValue_ForSameGuid()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var id1 = new FakeTypedId(guid);
            var id2 = new FakeTypedId(guid);

            // Act
            var hash1 = id1.GetHashCode();
            var hash2 = id2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentValue_ForDifferentGuids()
        {
            // Arrange
            var id1 = new FakeTypedId(Guid.NewGuid());
            var id2 = new FakeTypedId(Guid.NewGuid());

            // Act
            var hash1 = id1.GetHashCode();
            var hash2 = id2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}