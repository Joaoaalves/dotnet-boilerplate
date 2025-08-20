using Moq;
using Nest;
using Project.Domain.SharedKernel.Users;
using Project.Infrastructure.Logging.Users;
using Project.Logging.Users;

namespace Project.Tests.UnitTests.Infrastructure.LoggingTests.Users
{
    public class ElasticUserLoggerServiceTests
    {
        [Fact]
        public async Task LogUserCreated_ShouldCallElasticClientSuccessfully()
        {
            var mockClient = new Mock<IElasticClient>();
            var indexDef = new UserLogIndexDefinition();

            // Mock do IndexResponse válido
            var mockResponse = new Mock<IndexResponse>();
            mockResponse.Setup(r => r.IsValid).Returns(true);

            mockClient
                .Setup(c => c.IndexAsync(
                    It.IsAny<UserLogEntry>(),
                    It.IsAny<Func<IndexDescriptor<UserLogEntry>, IIndexRequest<UserLogEntry>>>(),
                    default
                ))
                .ReturnsAsync(mockResponse.Object);

            var service = new ElasticUserLoggerService(mockClient.Object, indexDef);
            var email = new Email("test@example.com");

            await service.LogUserCreated("user-1", email);
            await service.LogUserLoggedIn("user-1", email);
            await service.LogUserPasswordReset("user-1", email);

            mockClient.Verify(c => c.IndexAsync(
                It.IsAny<UserLogEntry>(),
                It.IsAny<Func<IndexDescriptor<UserLogEntry>, IIndexRequest<UserLogEntry>>>(),
                default), Times.Exactly(3));
        }

        [Fact]
        public async Task LogUserCreated_ShouldThrow_WhenElasticClientFails()
        {
            var mockClient = new Mock<IElasticClient>();
            var indexDef = new UserLogIndexDefinition();

            mockClient
                .Setup(c => c.IndexAsync(
                    It.IsAny<UserLogEntry>(),
                    It.IsAny<Func<IndexDescriptor<UserLogEntry>, IIndexRequest<UserLogEntry>>>(),
                    default
                ))
                .Throws(new Exception("fail"));

            var service = new ElasticUserLoggerService(mockClient.Object, indexDef);
            var email = new Email("test@example.com");

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                service.LogUserCreated("user-1", email));

            Assert.Equal("fail", ex.Message);
        }
    }
}
