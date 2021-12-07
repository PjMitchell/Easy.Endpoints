using Microsoft.AspNetCore.Http;

namespace Easy.Endpoints.TestService.Endpoints
{
    [EndpointController("Greetings")]
    public class HelloWorldEndpoint : IEndpoint
    {
        public string Handle(HttpContext ctx) => $"Hello World";
    }
}
