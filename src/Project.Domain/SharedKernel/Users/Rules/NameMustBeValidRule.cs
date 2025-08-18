using System.Text.RegularExpressions;
using Project.Domain.SeedWork;

namespace Project.Domain.SharedKernel.Users.Rules
{
    /// <summary>
    /// Business rule to ensure a name is valid.
    /// </summary>
    public partial class NameMustBeValidRule(string value) : IBusinessRule
    {
        private readonly string _value = value;

        /// <summary>
        /// Gets the validation message if the rule is broken.
        /// </summary>
        public string Message => "The name must be at least 3 characters long and contain only letters, and spaces.";

        /// <summary>
        /// Determines whether the rule is broken based on length and character constraints.
        /// </summary>
        /// <returns><c>true</c> if the name is invalid; otherwise, <c>false</c>.</returns>
        public bool IsBroken() =>
            string.IsNullOrWhiteSpace(_value) ||
            _value.Length < 3 ||
            !NameRegex().IsMatch(_value);

        /// <summary>
        /// Regex to validate name formatting (only letters and spaces).
        /// </summary>
        /// <returns>A compiled regular expression.</returns>
        [GeneratedRegex(@"^([\p{L}]+ ?)*$")]
        private static partial Regex NameRegex();
    }
}
