using Microsoft.AspNetCore.Identity;
using Project.Domain.Users;

namespace Project.Application.Users
{
    /// <summary>
    /// Defines the contract for user-related data operations in the domain layer.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>The user if found; otherwise, <c>null</c>.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>The user if found; otherwise, <c>null</c>.</returns>
        Task<User?> GetByIdAsync(UserId userId);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user if found; otherwise, <c>null</c>.</returns>
        Task<User?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Validates the provided password against the stored credentials for the specified user.
        /// </summary>
        /// <param name="user">The user to validate.</param>
        /// <param name="password">The plain-text password to check.</param>
        /// <returns><c>true</c> if the password is valid; otherwise, <c>false</c>.</returns>
        Task<bool> CheckPassowrd(User user, string password);

        /// <summary>
        /// Creates a new user with the specified password.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the result of the operation.</returns>
        Task<IdentityResult> CreateAsync(User user, string password);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user with updated data.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the result of the operation.</returns>
        Task<IdentityResult> UpdateAsync(User user);

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The identifier of the user to delete.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the result of the operation.</returns>
        Task<IdentityResult> DeleteAsync(UserId userId);
    }
}
