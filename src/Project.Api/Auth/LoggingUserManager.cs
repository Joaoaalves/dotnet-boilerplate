using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Project.Domain.SharedKernel.Users;
using Project.Domain.Users;
using Project.Logging.Users;

namespace Project.Api.Auth
{
    /// <summary>
    /// Custom implementation of <see cref="UserManager{TUser}"/> that adds logging capabilities
    /// for user account events such as password resets.
    /// </summary>
    public class LoggingUserManager : UserManager<User>
    {
        private readonly IUserLoggerService _userLoggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingUserManager"/> class.
        /// </summary>
        /// <param name="store">The user store used to persist user information.</param>
        /// <param name="optionsAccessor">Provides access to identity options.</param>
        /// <param name="passwordHasher">Hasher used to hash user passwords.</param>
        /// <param name="userValidators">List of user validators.</param>
        /// <param name="passwordValidators">List of password validators.</param>
        /// <param name="keyNormalizer">Used to normalize keys for consistent lookup.</param>
        /// <param name="errors">Provides identity error descriptions.</param>
        /// <param name="services">Service provider for dependency injection.</param>
        /// <param name="logger">The logger used for logging internal identity operations.</param>
        /// <param name="userLoggerService">Custom user logging service for tracking account events.</param>
        public LoggingUserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            IUserLoggerService userLoggerService
        ) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
                  keyNormalizer, errors, services, logger)
        {
            _userLoggerService = userLoggerService;
        }

        /// <summary>
        /// Resets the password for a user using the provided reset token and logs the operation if successful.
        /// </summary>
        /// <param name="user">The user whose password is being reset.</param>
        /// <param name="token">The password reset token.</param>
        /// <param name="newPassword">The new password to set for the user.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating whether the operation succeeded.</returns>
        /// <exception cref="Exception">Thrown when the user email is invalid or null.</exception>
        public override async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            var result = await base.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                var email = new Email(user.Email!) ?? throw new Exception("User with invalid e-mail was provided");
                await _userLoggerService.LogUserPasswordReset(user.Id, email);
            }

            return result;
        }
    }
}
