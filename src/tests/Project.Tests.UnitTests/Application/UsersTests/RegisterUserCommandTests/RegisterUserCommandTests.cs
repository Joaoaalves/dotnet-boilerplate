using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Project.Application.Users.Commands.RegisterUser;
using Project.Domain.SeedWork;
using Project.Domain.SharedKernel.Users;
using Project.Domain.Users;
using Project.Logging.Users;
using Project.Tests.UnitTests.Mocks;

namespace Project.Tests.UnitTests.Application.UsersTests.RegisterUserCommandTests
{
    public class RegisterUserCommandTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IUserLoggerService> _userLoggerMock;
        public RegisterUserCommandTests()
        {
            _userManagerMock = UserManagerMock.MockUserManager<User>([]);
            _userLoggerMock = new Mock<IUserLoggerService>();
        }

        [Fact]
        public async Task Handle_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var command = new RegisterUserCommand("John", "Doe", "john@doe.com", "P@ssword123");

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            var handler = new RegisterUserCommandHandler(_userManagerMock.Object, _userLoggerMock.Object);

            // Act
            var (userId, errors) = await handler.Handle(command, CancellationToken.None);

            // Assert
            userId.Should().NotBeNullOrEmpty();
            errors.Should().BeEmpty();

            _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<User>(), command.Password), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Once);
            _userLoggerMock.Verify(l => l.LogUserCreated(It.IsAny<string>(), It.IsAny<Email>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnErrors_WhenCreateFails()
        {
            // Arrange
            var command = new RegisterUserCommand("John", "Doe", "john@doe.com", "P@ssword123");

            var identityErrors = new List<IdentityError>
            {
                new IdentityError { Description = "Email already taken" }
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

            var handler = new RegisterUserCommandHandler(_userManagerMock.Object, _userLoggerMock.Object);

            // Act
            var (userId, errors) = await handler.Handle(command, CancellationToken.None);

            // Assert
            userId.Should().BeEmpty();
            errors.Should().ContainSingle().Which.Should().Be("Email already taken");

            _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<User>(), command.Password), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Never);
            _userLoggerMock.Verify(l => l.LogUserCreated(It.IsAny<string>(), It.IsAny<Email>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldTrimNamesAndEmail()
        {
            // Arrange
            var command = new RegisterUserCommand("  John  ", "  Doe  ", "  john@doe.com  ", "P@ssword123");

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            var handler = new RegisterUserCommandHandler(_userManagerMock.Object, _userLoggerMock.Object);

            // Act
            var (userId, errors) = await handler.Handle(command, CancellationToken.None);

            // Assert
            userId.Should().NotBeNullOrEmpty();
            errors.Should().BeEmpty();

            _userManagerMock.Verify(um => um.CreateAsync(It.Is<User>(u =>
                u.FirstName.Value == "John" &&
                u.LastName.Value == "Doe" &&
                u.Email == "john@doe.com"
            ), command.Password), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowOnInvalidName()
        {
            // Arrange
            var command = new RegisterUserCommand("J", "Doe", "john@doe.com", "P@ssword123");

            var handler = new RegisterUserCommandHandler(_userManagerMock.Object, _userLoggerMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessRuleValidationException>()
                .WithMessage("The name must be at least 3 characters long and contain only letters, and spaces.");
        }

        [Fact]
        public async Task Handle_ShouldThrowOnInvalidEmail()
        {
            // Arrange
            var command = new RegisterUserCommand("John", "Doe", "invalid-email", "P@ssword123");

            var handler = new RegisterUserCommandHandler(_userManagerMock.Object, _userLoggerMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessRuleValidationException>()
                .WithMessage("The email must be a valid email address.");
        }
    }
}