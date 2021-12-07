using Microsoft.AspNetCore.Http;

namespace Easy.Endpoints.TestService.Endpoints
{
    [EndpointController("Greetings")]
    public class GetGreeterEndpoint : IEndpoint
    {
        public string Handle(HttpContext ctx, [FromHeader("x-target")] string target) => $"Hello {target}";
    }
}
