using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Project.Domain.SharedKernel.Users;
using Project.Domain.Users;
using Project.Logging.Users;

namespace Project.Api.Auth
{
    /// <summary>
    /// Custom implementation of <see cref="SignInManager{TUser}"/> that adds logging capabilities
    /// for user sign-in events.
    /// </summary>
    public class LoggingSignInManager : SignInManager<User>
    {
        private readonly IUserLoggerService _userLoggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingSignInManager"/> class.
        /// </summary>
        /// <param name="userManager">The user manager used to manage user accounts.</param>
        /// <param name="contextAccessor">Provides access to the current HTTP context.</param>
        /// <param name="claimsFactory">Factory to create claims principals for users.</param>
        /// <param name="optionsAccessor">Provides access to identity options.</param>
        /// <param name="logger">The logger used for logging user operations.</param>
        /// <param name="schemes">Authentication scheme provider.</param>
        /// <param name="confirmation">User confirmation service.</param>
        /// <param name="userLoggerService">Custom user logging service for tracking sign-in events.</param>
        public LoggingSignInManager(
            UserManager<User> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<User> confirmation,
            IUserLoggerService userLoggerService
        )
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _userLoggerService = userLoggerService;
        }

        /// <summary>
        /// Checks if the specified password is valid for the given user and logs the login event if successful.
        /// </summary>
        /// <param name="user">The user whose password is being checked.</param>
        /// <param name="password">The password to validate.</param>
        /// <param name="lockoutOnFailure">Whether to lock the user out on failure.</param>
        /// <returns>A <see cref="SignInResult"/> indicating the result of the password validation.</returns>
        /// <exception cref="Exception">Thrown when the user email is invalid or null.</exception>
        public override async Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
        {
            var result = await base.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            if (result.Succeeded)
            {
                var email = new Email(user.Email!) ?? throw new Exception("User with invalid e-mail was provided");
                await _userLoggerService.LogUserLoggedIn(user.Id, email);
            }

            return result;
        }
    }
}
