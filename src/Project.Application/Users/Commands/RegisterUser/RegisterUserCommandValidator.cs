using FluentValidation;
using Project.Domain.SharedKernel.Users.Rules;

namespace Project.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Must(BeAValidName).WithMessage(BeAValidNameMessage());

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Must(BeAValidName).WithMessage(BeAValidNameMessage());

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one number")
                .Matches(@"[^\da-zA-Z]").WithMessage("Password must contain at least one special character");
        }

        private static bool BeAValidName(string value)
        {
            var rule = new NameMustBeValidRule(value);
            return !rule.IsBroken();
        }

        private static string BeAValidNameMessage()
        {
            var rule = new NameMustBeValidRule("");
            return rule.Message;
        }
    }

}