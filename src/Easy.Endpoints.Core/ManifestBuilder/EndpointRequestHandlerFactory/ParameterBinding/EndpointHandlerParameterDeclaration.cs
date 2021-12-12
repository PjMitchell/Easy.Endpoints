using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Information about endpoint handler parameter 
    /// </summary>
    public abstract class EndpointHandlerParameterDeclaration
    {
        /// <summary>
        /// Gets descriptors for parameters
        /// </summary>
        /// <returns>Descriptors for parameters</returns>
        public abstract IEnumerable<EndpointParameterDescriptor> GetParameterDescriptors();

        /// <summary>
        /// Factory for building parameter from HttpContext
        /// </summary>
        public abstract ValueTask<object?> Factory(HttpContext httpContext, EndpointOptions options);
    }
}
