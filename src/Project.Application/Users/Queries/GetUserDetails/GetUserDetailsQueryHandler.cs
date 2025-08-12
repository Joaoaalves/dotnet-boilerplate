using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Project.Application.Configuration.Queries;
using Project.Application.Users.Mappers;
using Project.Domain.Users;

namespace Project.Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQueryHandler(
        UserManager<User> userManager
    ) : IQueryHandler<GetUserDetailsQuery, UserDetailsDTO>
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<UserDetailsDTO> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(request.Principal), cancellationToken: cancellationToken) ?? throw new Exception("Invalid user!");
            return user.ToUserDetailsDTO();
        }
    }
}