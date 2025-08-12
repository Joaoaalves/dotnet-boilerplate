using System.Net.Mail;
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
                return addr.Address != _email;
            }
            catch
            {
                return true;
            }
        }
    }
}
