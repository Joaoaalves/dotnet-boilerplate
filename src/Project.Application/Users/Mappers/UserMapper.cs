using Project.Application.Users.Queries.GetUserDetails;
using Project.Domain.Users;

namespace Project.Application.Users.Mappers
{
    public static class UserMapper
    {
        public static UserDetailsDTO ToUserDetailsDTO(this User user)
        {
            return new UserDetailsDTO
            {
                FirstName = user.FirstName.Value,
                LastName = user.LastName.Value,
                Email = user.Email ?? ""
            };
        }
    }
}