using System.Security.Claims;
using Project.Application.Configuration.Queries;

namespace Project.Application.Users.Queries.GetUserDetails
{
    public class GetUserDetailsQuery(
        ClaimsPrincipal claimsPrincipal
    ) : IQuery<UserDetailsDTO>
    {
        public ClaimsPrincipal Principal { get; } = claimsPrincipal;
    }
}