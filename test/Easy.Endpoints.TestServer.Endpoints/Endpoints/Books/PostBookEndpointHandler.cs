using System.Threading.Tasks;

namespace Easy.Endpoints.TestServer.Endpoints.Books
{

    public class PostBookEndpointHandler : IJsonEndpointHandler<Book, CommandResult>
    {
        public PostBookEndpointHandler()
        {            
        }

        public Task<CommandResult> Handle(Book body)
        {
            return Task.FromResult(new CommandResult { Successful = true, Message = $"Created {body.Name}" });
        }
    }
}
