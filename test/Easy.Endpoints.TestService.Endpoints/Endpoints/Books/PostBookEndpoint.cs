using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Books
{
    public class PostBookEndpoint : IEndpoint
    {
        public Task<CommandResult> HandleAsync(Book body, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CommandResult { Successful = true, Message = $"Created {body.Name}" });
        }
    }
}
