
using Microsoft.AspNetCore.Identity;
using Project.Application.Users;
using Project.Domain.Users;

namespace Project.Tests.UnitTests.Infrastructure.Fakes
{
    public class FakeUserRepository(User? userToReturn = null) : IUserRepository
    {
        private User? _userToReturn = userToReturn;

        public Task<bool> CheckPassowrd(User user, string password)
        {
            if (_userToReturn == null || user == null)
                return Task.FromResult(false);

            return Task.FromResult(_userToReturn.PasswordHash == user.PasswordHash);
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            _userToReturn = user;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(string userId)
        {
            _userToReturn = null;
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return Task.FromResult(_userToReturn);
        }

        public Task<User?> GetByIdAsync(string userId)
        {
            return Task.FromResult(_userToReturn);
        }

        public Task<User?> GetUserByUsernameAsync(string username)
        {
            return Task.FromResult(_userToReturn);
        }

        public Task<IdentityResult> UpdateAsync(User user)
        {
            _userToReturn = user;
            return Task.FromResult(IdentityResult.Success);
        }
    }
}