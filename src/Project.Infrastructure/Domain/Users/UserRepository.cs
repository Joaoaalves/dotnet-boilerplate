using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Project.Domain.Users;

using Project.Infrastructure.Database;

using Project.Application.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Project.Infrastructure.Domain.Users
{
    /// <summary>
    /// Provides an implementation of <see cref="IUserRepository"/> using Entity Framework Core and ASP.NET Identity.
    /// </summary>
    public class UserRepository(
        UserManager<User> userManager,
        DbContext applicationDbContext
    ) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly DbContext _dbContext = applicationDbContext;

        /// <inheritdoc />
        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        /// <inheritdoc />
        public async Task<IdentityResult> DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            return await _userManager.DeleteAsync(user);
        }

        /// <inheritdoc />
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        /// <inheritdoc />
        public async Task<User?> GetByIdAsync(string userId)
        {
            return await _dbContext.Set<User>()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <inheritdoc />
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        /// <inheritdoc />
        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        /// <inheritdoc />
        public async Task<bool> CheckPassowrd(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}

