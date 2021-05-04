using System.Collections.Generic;
using System.Text.Json;

namespace Easy.Endpoints
{
    /// <summary>
    /// Option for Endpoints
    /// </summary>
    public class EndpointOptions
    {
        /// <summary>
        /// Creates new instance of EndpointOptions with default values
        /// </summary>
        public EndpointOptions()
        {
            EndpointForHandlerDeclarations = new List<IEndpointForHandlerDeclaration>
            {
                new JsonEndpointForHandlerDeclarations()
            };
            JsonSerializerOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
        }

        /// <summary>
        /// Route Pattern when none is specified, defaults "[endpoint]"
        /// </summary>
        public string RoutePattern { get; internal set; } = "[endpoint]";

        /// <summary>
        /// Json Serializer Options for Endpoints
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; internal set; }

        /// <summary>
        /// All EndpointForHandlerDeclaration
        /// </summary>
        public IReadOnlyList<IEndpointForHandlerDeclaration> EndpointForHandlerDeclarations { get; internal set; }
    }
}
