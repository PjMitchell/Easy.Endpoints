using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Auth
{
    [Authorize]
    public class UserInfoEndpointHandler : IEndpoint
    {
        public Task<AuthRequest> HandleAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            return Task.FromResult(FromPrincipal(user));
        }

        private static AuthRequest FromPrincipal(ClaimsPrincipal user)
        {
            var name = user.Identity.Name;
            var roles = user.Claims.Where(r => r.Type == AuthService.RoleClaimType).Select(s => s.Value).ToArray();
            return new AuthRequest { Roles = roles, Username = name };
        }
    }
}
