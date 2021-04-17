using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestServer.Endpoints
{
    [EndpointController("Greetings")]
    public class HelloWorldEndpoint : IEndpoint
    {
        public Task HandleRequest(HttpContext httpContext)
        {
            httpContext.Response.WriteAsync("Hello World");
            return Task.CompletedTask;
        }
    }
}
