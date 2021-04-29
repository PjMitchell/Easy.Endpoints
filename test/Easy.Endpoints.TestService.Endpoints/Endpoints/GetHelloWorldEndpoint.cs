using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints
{
    [EndpointController("Greetings")]
    public class HelloWorldEndpoint : IEndpoint
    {
        public Task HandleRequest(EndpointContext endpointContext)
        {
            endpointContext.Response.WriteAsync("Hello World");
            return Task.CompletedTask;
        }
    }
}
