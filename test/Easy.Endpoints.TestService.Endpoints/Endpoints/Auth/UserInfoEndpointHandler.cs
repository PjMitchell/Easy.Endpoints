using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Auth
{
    [Authorize]
    public class UserInfoEndpointHandler : IJsonResponseEndpointHandler<AuthRequest>
    {
        private readonly IEndpointContextAccessor contextAccessor;

        public UserInfoEndpointHandler(IEndpointContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public Task<AuthRequest> HandleAsync(CancellationToken cancellationToken)
        {
            var context = contextAccessor.GetContext();
            return Task.FromResult(FromPrincipal(context.User));
        }

        private AuthRequest FromPrincipal(ClaimsPrincipal user)
        {
            var name = user.Identity.Name;
            var roles = user.Claims.Where(r => r.Type == AuthService.RoleClaimType).Select(s => s.Value).ToArray();
            return new AuthRequest { Roles = roles, Username = name };
        }
    }
}
