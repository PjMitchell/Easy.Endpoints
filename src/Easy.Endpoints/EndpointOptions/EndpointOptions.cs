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
                new EndpointsForEndpointResultHandlerDeclarations(),
                new JsonEndpointForHandlerDeclarations()
            };
            JsonSerializerOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
            EndpointMetaDeclarations = new List<IEndpointMetaDataDeclaration>
            {
                new AuthEndpointMetaDataDeclaration()
            };
        }

        /// <summary>
        /// Route Pattern when none is specified, defaults "[endpoint]"
        /// </summary>
        public string RoutePattern { get; internal set; } = "[endpoint]";

        /// <summary>
        /// JSON Serializer Options for Endpoints
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; internal set; }

        /// <summary>
        /// All EndpointForHandlerDeclarations
        /// </summary>
        public IReadOnlyList<IEndpointForHandlerDeclaration> EndpointForHandlerDeclarations { get; internal set; }

        /// <summary>
        /// All EndpointMetaDeclarations 
        /// </summary>
        public IReadOnlyList<IEndpointMetaDataDeclaration> EndpointMetaDeclarations { get; internal set; }

        
    }
}
