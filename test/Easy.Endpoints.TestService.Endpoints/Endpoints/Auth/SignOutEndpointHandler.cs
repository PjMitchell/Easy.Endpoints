using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Auth
{
    public class SignOutEndpointHandler : IEndpointResultHandler
    {
        private readonly IAuthService authService;

        public SignOutEndpointHandler(IAuthService authService)
        {
            this.authService = authService;
        }
        public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken)
        {
            authService.SignOut();
            return Task.FromResult<IEndpointResult>(new NoContentResult());
        }
    }
}
