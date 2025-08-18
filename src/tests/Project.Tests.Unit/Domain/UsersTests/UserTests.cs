using FluentAssertions;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;
using Project.Tests.Unit.Builders;


namespace Project.Tests.Unit.Domain.UsersTests
{
    public class UserTests
    {
        [Fact]
        public void ShouldCreateUserWithValidData()
        {
            var user = UserBuilder.WithDefaultValues();

            user.Should().NotBeNull();
            user.FirstName.Value.Should().Be("John");
            user.LastName.Value.Should().Be("Doe");
            user.Email.Should().Be("john@doe.com");
            user.UserName.Should().Be("john@doe.com");
        }


        [Fact]
        public void ShouldAllowRenamingUser()
        {
            var user = UserBuilder.WithDefaultValues();

            var newFirstName = new Name("Éric");
            var newLastName = new Name("Dupont");

            user.Rename(newFirstName, newLastName);

            user.FirstName.Value.Should().Be("Éric");
            user.LastName.Value.Should().Be("Dupont");
        }

        [Fact]
        public void ShouldAllowPartialRenaming()
        {
            var user = UserBuilder.WithDefaultValues();

            var newFirstName = new Name("Alice");

            user.Rename(newFirstName, null);

            user.FirstName.Value.Should().Be("Alice");
            user.LastName.Value.Should().Be("Doe"); // unchanged
        }


        [Fact]
        public void UsersWithSameDataShouldBeEqual()
        {
            var user1 = UserBuilder.WithDefaultValues();
            var user2 = UserBuilder.WithDefaultValues();

            user1.Should().NotBeSameAs(user2);
            user1.FirstName.Should().Be(user2.FirstName);
            user1.LastName.Should().Be(user2.LastName);
            user1.Email.Should().Be(user2.Email);
            user1.UserName.Should().Be(user2.UserName);
        }

        [Fact]
        public void UsersWithDifferentDataShouldNotBeEqual()
        {
            var user1 = UserBuilder.WithDefaultValues();
            var user2 = new UserBuilder()
                .WithName(new Name("Jane"), new Name("Smith"))
                .WithEmail(new Email("jane@smith.com"))
                .WithUserName(new UserName("jane@smith.com"))
                .Build();

            user1.FirstName.Should().NotBe(user2.FirstName);
            user1.LastName.Should().NotBe(user2.LastName);
            user1.Email.Should().NotBe(user2.Email);
            user1.UserName.Should().NotBe(user2.UserName);
        }
    }
}
