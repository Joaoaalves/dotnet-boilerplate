using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Project.Tests.UnitTests.Application.Fakes
{
    public class FakeHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext? HttpContext { get; set; }

        public static FakeHttpContextAccessor WithEmail(string email)
        {
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email)
            }, "mock"));

            return new FakeHttpContextAccessor
            {
                HttpContext = new DefaultHttpContext { User = claims }
            };
        }

        public static FakeHttpContextAccessor Empty()
        {
            return new FakeHttpContextAccessor
            {
                HttpContext = null
            };
        }
    }
}