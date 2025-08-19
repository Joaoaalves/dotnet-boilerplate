using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Project.Domain.SeedWork;

namespace Project.Domain.SharedKernel.Users.Rules
{
    /// <summary>
    /// Business rule to ensure a name is valid.
    /// </summary>
    public sealed class NameMustBeValidRule(string value) : IBusinessRule
    {
        private readonly string _value = value;

        /// <summary>
        /// Gets the validation message if the rule is broken.
        /// </summary>
        public string Message => "The name must be at least 3 characters long and contain only letters, and spaces.";

        // We keep the regex pre-compiled and culture-invariant.
        // A small timeout protects against catastrophic backtracking on pathological inputs.
        private static readonly Regex _nameRegex = new(
            @"^([\p{L}]+ ?)*$");

        /// <summary>
        /// Determines whether the rule is broken based on length and character constraints.
        /// </summary>
        public bool IsBroken() =>
            string.IsNullOrWhiteSpace(_value) ||
            _value.Length < 3 ||
            !_nameRegex.IsMatch(_value);
    }
}
