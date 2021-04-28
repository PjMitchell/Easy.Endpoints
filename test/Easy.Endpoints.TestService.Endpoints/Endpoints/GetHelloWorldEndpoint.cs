using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints
{
    [EndpointController("Greetings")]
    public class HelloWorldEndpoint : IEndpoint
    {
        public Task HandleRequest(EndpointContext httpContext)
        {
            httpContext.HttpContext.Response.WriteAsync("Hello World");
            return Task.CompletedTask;
        }
    }
}
