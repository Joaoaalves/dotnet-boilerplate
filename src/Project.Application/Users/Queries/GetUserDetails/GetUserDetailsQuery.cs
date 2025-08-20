using Project.Application.Configuration.Queries;

namespace Project.Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQuery(
        string? email = null
    ) : IQuery<UserDetailsDTO?>
    {
        public string? Email { get; } = email;
    }
}