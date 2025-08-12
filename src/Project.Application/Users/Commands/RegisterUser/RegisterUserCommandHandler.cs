using Microsoft.AspNetCore.Identity;
using Project.Application.Configuration.Commands;
using Project.Domain.SharedKernel.Users;
using Project.Domain.Users;

namespace Project.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler(
        UserManager<User> userManager
    ) : ICommandHandler<RegisterUserCommand, (string userId, IEnumerable<string> Errors)>
    {
        private readonly UserManager<User> _userManager = userManager;
        public async Task<(string userId, IEnumerable<string> Errors)> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var firstName = new Name(request.FirstName);
            var lastName = new Name(request.LastName);
            var email = new Email(request.Email);
            var userName = new UserName(request.Email);

            var user = User.Create(firstName, lastName, userName, email);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return (string.Empty, result.Errors.Select(e => e.Description));

            await _userManager.UpdateAsync(user);

            return (user.Id, []);
        }
    }
}