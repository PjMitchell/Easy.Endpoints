using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <summary>
    /// Returns GenericEndpointTypeInfo for an endpoint
    /// </summary>
    public interface IGenericEndpointTypeInfoProvider
    {
        /// <summary>
        /// Gets GenericEndpointTypeInfo
        /// </summary>
        /// <returns>All generic Endpoint type info for an endpoint</returns>
        IEnumerable<IGenericEndpointTypeInfo> GetGenericEndpointTypeInfo();
    }
}
