using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Easy.Endpoints
{
    /// <summary>
    /// Provides the context for an endpoint request
    /// </summary>
    public abstract class EndpointContext : HttpContext
    {
        /// <summary>
        /// Json Serializer Options for Endpoints
        /// </summary>
        public abstract JsonSerializerOptions JsonSerializerOptions { get; }
    }
}
