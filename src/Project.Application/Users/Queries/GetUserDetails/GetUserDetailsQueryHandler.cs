using Project.Application.Configuration.Queries;
using Project.Application.Users.Mappers;

namespace Project.Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQueryHandler(
        IUserRepository userRepository
    ) : IQueryHandler<GetUserDetailsQuery, UserDetailsDTO?>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserDetailsDTO?> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var email = request.Email;

            if (string.IsNullOrEmpty(email))
                return null;

            var user = await _userRepository.GetByEmailAsync(email);

            return user?.ToUserDetailsDTO();
        }

    }
}