using System.Collections.Generic;

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
        }

        /// <summary>
        /// Route Pattern when none is specified, defaults "[endpoint]"
        /// </summary>
        public string RoutePattern { get; internal set; } = "[endpoint]";

        /// <summary>
        /// All EndpointForHandlerDeclaration 
        /// </summary>
        public IReadOnlyList<IEndpointForHandlerDeclaration> EndpointForHandlerDeclarations { get; internal set; }
    }
}
