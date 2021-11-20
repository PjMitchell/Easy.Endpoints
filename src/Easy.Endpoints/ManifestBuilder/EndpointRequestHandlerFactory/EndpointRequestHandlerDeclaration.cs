using Microsoft.AspNetCore.Http;
using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Information about Endpoint Request Handler. Includes details like Return type and Parameters.
    /// </summary>
    public class EndpointRequestHandlerDeclaration
    {
        /// <summary>
        /// Creates new instance of EndpointRequestHandlerDeclaration
        /// </summary>
        /// <param name="factory">EndpointRequestHandlerFactory for endpoint</param>
        /// <param name="parameterInfos">EndpointParameterInfos for endpoint</param>
        /// <param name="returnType">Return type of endpoint if know. IEndpointResult will return null</param>
        public EndpointRequestHandlerDeclaration(EndpointRequestHandlerFactory factory, EndpointParameterInfo[] parameterInfos, Type? returnType = null)
        {
            Factory = factory;
            ParameterInfos = parameterInfos;
            ReturnType = returnType;
        }

        /// <summary>
        /// Factory for EndpointRequestHandler
        /// </summary>
        public EndpointRequestHandlerFactory Factory { get; }

        /// <summary>
        /// Return type of endpoint if know. IEndpointResult will return null
        /// </summary>
        public Type? ReturnType { get; }

        /// <summary>
        /// Parameters for endpoint
        /// </summary>
        public EndpointParameterInfo[] ParameterInfos { get; }

    }

    /// <summary>
    /// Generates IEndpointRequestHandler for endpoint from HttpContext
    /// </summary>
    /// <param name="context">Request HttpContext</param>
    /// <returns>IEndpointRequestHandler for endpoint</returns>
    public delegate IEndpointRequestHandler EndpointRequestHandlerFactory(HttpContext context);
}
