using System.Text.RegularExpressions;
using Project.Domain.SeedWork;

namespace Project.Domain.SharedKernel.Users.Rules
{
    /// <summary>
    /// Business rule to ensure a username is valid.
    /// </summary>
    public partial class UserNameMustBeValidRule(string userName) : IBusinessRule
    {
        private readonly string _userName = userName;

        /// <summary>
        /// Gets the validation message if the rule is broken.
        /// </summary>
        public string Message => "The username must be at least 3 characters long and can only contain letters, numbers, and underscores.";

        /// <summary>
        /// Determines whether the rule is broken based on length and allowed characters.
        /// </summary>
        /// <returns><c>true</c> if the username is invalid; otherwise, <c>false</c>.</returns>
        public bool IsBroken() =>
            string.IsNullOrWhiteSpace(_userName) ||
            _userName.Length < 3 ||
            !_userNameRegex.IsMatch(_userName);

        /// <summary>
        /// Regex to validate username format (letters, numbers, underscores).
        /// </summary>
        private static readonly Regex _userNameRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    }
}
