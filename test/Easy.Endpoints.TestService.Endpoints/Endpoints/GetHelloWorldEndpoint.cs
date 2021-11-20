using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints.TestService.Endpoints
{
    [EndpointController("Greetings")]
    public class HelloWorldEndpoint : IEndpoint
    {
        public async Task<IEndpointResult> HandleAsync(HttpContext ctx)
        {
            ctx.Response.StatusCode = 200;
            await ctx.Response.WriteAsync("Hello World");
            return EndpointResult.Completed();
        }
    }
}
