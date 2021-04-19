using Microsoft.AspNetCore.Http;

namespace Easy.Endpoints
{
    /// <summary>
    /// Provides the context for an endpoint request
    /// </summary>
    public class EndpointContext
    {
        /// <summary>
        /// Created a new instance of the Endpoint Context
        /// </summary>
        /// <param name="context">HttpContext for the request</param>
        public EndpointContext(HttpContext context)
        {
            HttpContext = context;
        }

        /// <summary>
        /// HttpContext for the request
        /// </summary>
        public HttpContext HttpContext { get; }
    }
}
