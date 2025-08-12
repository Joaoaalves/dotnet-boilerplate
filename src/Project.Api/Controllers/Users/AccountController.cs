using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Project.Application.Users.Queries.GetUserDetails;
using Project.Application.Users.Commands.RegisterUser;

using Project.Infrastructure.Processing;

namespace Project.Api.Controllers.Users
{
    /// <summary>
    /// Controller responsible for handling user account-related endpoints such as registration and user info retrieval.
    /// </summary>
    [ApiController]
    [Route("/api/")]
    public class AccountController(
        QueriesExecutor queriesExecutor,
        CommandsExecutor commandsExecutor
    ) : ControllerBase
    {
        private readonly QueriesExecutor _queriesExecutor = queriesExecutor;
        private readonly CommandsExecutor _commandsExecutor = commandsExecutor;

        /// <summary>
        /// Registers a new user with the provided registration data.
        /// </summary>
        /// <param name="command">The registration command containing user details.</param>
        /// <returns>
        /// Returns 200 OK if registration is successful.
        /// Returns 400 Bad Request if validation fails or errors occur during registration.
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (userId, errors) = await _commandsExecutor.Execute(command);

            if (errors.Any())
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);
                return BadRequest(ModelState);
            }

            return Ok(new { Message = "User registered successfully" });
        }

        /// <summary>
        /// Retrieves information about the currently authenticated user.
        /// </summary>
        /// <returns>
        /// Returns 200 OK with user details if authenticated.
        /// Returns 401 Unauthorized if the user is not authenticated or not found.
        /// </returns>
        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var query = new GetUserDetailsQuery(User);

            var dto = await _queriesExecutor.Execute(query);

            return dto is null ? Unauthorized() : Ok(dto);
        }
    }

}
