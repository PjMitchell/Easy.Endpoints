using Microsoft.AspNetCore.Http;

namespace Easy.Endpoints
{
    public interface IEndpointContextAccessor
    {
        EndpointContext? Context { get; }
    }

    public class EndpointContextAccessor : IEndpointContextAccessor
    {
        public EndpointContext? Context { get; set; }
    }

    public class EndpointContext
    {
        public EndpointContext(HttpContext context)
        {
            HttpContext = context;
        }

        public HttpContext HttpContext { get; }
    }
}
