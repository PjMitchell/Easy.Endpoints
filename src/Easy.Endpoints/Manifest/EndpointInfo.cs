using Microsoft.AspNetCore.Routing.Patterns;
using System;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <summary>
    /// Information about a Easy.Endpoint
    /// </summary>
    public class EndpointInfo
    {
        /// <summary>
        /// Constructs new instance of EnpointInfo
        /// </summary>
        /// <param name="type">Type of IEndpoint</param>
        /// <param name="handlerDeclaration">Information about the endpoint handler</param>
        /// <param name="pattern">Route pattern of endpoint</param>
        /// <param name="name">Route Name</param>
        /// <param name="order">Route order.</param>
        public EndpointInfo(Type type, EndpointRequestHandlerDeclaration handlerDeclaration,  RoutePattern pattern, string name, int order)
        {
            Type = type;
            HandlerDeclaration = handlerDeclaration;
            Pattern = pattern;
            Order = order;
            Name = name;
            Meta = new List<object>();
        }

        /// <summary>
        /// Gets Type of IEndpoint
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets route pattern of endpoint
        /// </summary>
        public RoutePattern Pattern { get; }

        /// <summary>
        /// Information on the Endpoint Request Handler
        /// </summary>
        public EndpointRequestHandlerDeclaration HandlerDeclaration { get; }

        /// <summary>
        /// Gets Route Name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets Route order
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Gets Meta data for endpoint
        /// </summary>
        public IList<object> Meta { get; }
    }
}
