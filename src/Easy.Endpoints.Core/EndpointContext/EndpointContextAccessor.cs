using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Gets Endpoint Context, accessor is scoped to the request
    /// </summary>
    public interface IEndpointContextAccessor
    {
        /// <summary>
        /// Gets Context
        /// </summary>
        /// <returns>Returns context for request</returns>
        EndpointContext GetContext();
    }

}
