using System.Net.Mail;
using System.Text.RegularExpressions;
using Project.Domain.SeedWork;

namespace Project.Domain.SharedKernel.Users.Rules
{
    /// <summary>
    /// Business rule to ensure an email address is valid.
    /// </summary>
    public class EmailMustBeValidRule(string email) : IBusinessRule
    {
        private readonly string _email = email;

        /// <summary>
        /// Gets the validation message if the rule is broken.
        /// </summary>
        public string Message => "The email must be a valid email address.";

        /// <summary>
        /// Determines whether the rule is broken based on the email format.
        /// </summary>
        /// <returns><c>true</c> if the email is invalid; otherwise, <c>false</c>.</returns>
        public bool IsBroken()
        {
            if (string.IsNullOrWhiteSpace(_email))
                return true;

            try
            {
                var addr = new MailAddress(_email);
                if (addr.Address != _email)
                    return true;

                // Extract domain part after '@'
                var domain = _email.Split('@')[1];

                // Ensure domain has at least one dot and TLD of 2+ characters
                var domainParts = domain.Split('.');
                if (domainParts.Length < 2 || domainParts[^1].Length < 2)
                    return true;

                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
