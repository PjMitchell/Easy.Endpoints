using Microsoft.AspNetCore.Authorization;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Auth
{
    [AllowAnonymous]
    [Post]
    public class SignInEndpoint : IEndpoint
    {
        private readonly IAuthService authService;

        public SignInEndpoint(IAuthService authService)
        {
            this.authService = authService;
        }

        public Task HandleAsync(AuthRequest body, CancellationToken cancellationToken)
        {
            authService.SignIn(body.Username, body.Roles);
            return Task.CompletedTask;
        }
    }
}
