using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Project.Domain.Users;

using Project.Infrastructure.Users;
using Project.Infrastructure.Database;

using Project.Application.Users;

namespace Project.Infrastructure.Domain.Users
{
    /// <summary>
    /// Provides an implementation of <see cref="IUserRepository"/> using Entity Framework Core and ASP.NET Identity.
    /// </summary>
    public class UserRepository(
        UserManager<IdentityUserAdapter> userManager,
        ApplicationDbContext applicationDbContext
    ) : IUserRepository
    {
        private readonly UserManager<IdentityUserAdapter> _userManager = userManager;
        private readonly ApplicationDbContext _dbContext = applicationDbContext;

        /// <inheritdoc />
        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var identityUser = IdentityUserAdapter.FromDomain(user);
            return await _userManager.CreateAsync(identityUser, password);
        }

        /// <inheritdoc />
        public async Task<IdentityResult> DeleteAsync(UserId userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()!);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            return await _userManager.DeleteAsync(user);
        }

        /// <inheritdoc />
        public async Task<User?> GetByEmailAsync(string email)
        {
            var identityUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return identityUser?.ToDomain();
        }

        /// <inheritdoc />
        public async Task<User?> GetByIdAsync(UserId userId)
        {
            var identityUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId.ToString());

            return identityUser?.ToDomain();
        }

        /// <inheritdoc />
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var identityUser = await _userManager.FindByNameAsync(username);

            return identityUser?.ToDomain();
        }

        /// <inheritdoc />
        public async Task<IdentityResult> UpdateAsync(User user)
        {
            var identityUser = IdentityUserAdapter.FromDomain(user);

            return await _userManager.UpdateAsync(identityUser);

        }

        /// <inheritdoc />
        public async Task<bool> CheckPassowrd(User user, string password)
        {
            var identityUser = IdentityUserAdapter.FromDomain(user);

            return await _userManager.CheckPasswordAsync(identityUser, password);
        }
    }
}
