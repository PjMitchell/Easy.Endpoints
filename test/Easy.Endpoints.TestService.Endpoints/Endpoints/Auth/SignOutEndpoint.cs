using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Auth
{
    public class SignOutEndpoint : IEndpoint
    {
        private readonly IAuthService authService;

        public SignOutEndpoint(IAuthService authService)
        {
            this.authService = authService;
        }
        public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken)
        {
            authService.SignOut();
            return Task.FromResult(EndpointResult.NoContent());
        }
    }
}
