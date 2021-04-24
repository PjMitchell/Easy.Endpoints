using System;
using System.Reflection;

namespace Easy.Endpoints
{
    /// <summary>
    /// Gets Endpoint type for EndpointHandler Type
    /// </summary>
    public interface IEndpointForHandlerDeclaration
    {
        /// <summary>
        /// Gets Endpoint type for EndpointHandler Type
        /// </summary>
        /// <param name="handlerTypeInfo">EndpointHandler TypeInfo</param>
        /// <returns>Endpoint type for EndpointHandler Type, returns null if no valid endpoint found</returns>
        Type? GetEndpointForHandler(TypeInfo handlerTypeInfo);
    }
}
