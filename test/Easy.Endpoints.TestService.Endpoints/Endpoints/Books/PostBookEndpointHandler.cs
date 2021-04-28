using System.Threading;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints.Books
{

    public class PostBookEndpointHandler : IJsonEndpointHandler<Book, CommandResult>
    {
        public PostBookEndpointHandler()
        {            
        }

        public Task<CommandResult> Handle(Book body, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CommandResult { Successful = true, Message = $"Created {body.Name}" });
        }
    }
}
