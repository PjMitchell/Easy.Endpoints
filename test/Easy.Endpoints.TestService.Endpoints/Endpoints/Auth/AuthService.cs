using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Auth
{
    public interface IAuthService
    {
        void SignIn(string username, string[] roles);
        ClaimsPrincipal GetCurrent();
        void SignOut();
    }
    public class AuthService : IAuthService
    {
        private const string userClaimType = "username";
        public const string RoleClaimType = "role";

        private ClaimsPrincipal currentUser;

        public AuthService()
        {
            currentUser = new ClaimsPrincipal();
        }

        public void SignIn(string username, string[] roles)
        {
            var usernameClaim = new Claim(userClaimType, username);
            var roleClaims = roles.Select(s => new Claim(RoleClaimType, s)).ToArray();
            var allClaims = roleClaims.Concat(new[] { usernameClaim });
            currentUser = new ClaimsPrincipal(new ClaimsIdentity(allClaims, TestAuthenticationHandler.Schema, userClaimType, RoleClaimType));
        }

        public ClaimsPrincipal GetCurrent() => currentUser;

        public void SignOut()
        {
            currentUser = new ClaimsPrincipal();
        }

    }

    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthOptions>
    {
        public const string Schema = "DevAuth";
        private readonly IAuthService authService;

        public TestAuthenticationHandler(IAuthService authService, IOptionsMonitor<TestAuthOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock) : base(options, loggerFactory, encoder, clock)
        {
            this.authService = authService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(authService.GetCurrent(), Scheme.Name)));
        }

        
    }

    public class TestAuthOptions : AuthenticationSchemeOptions { }
}
