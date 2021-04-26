using System;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <summary>
    /// Gets Type info and route values for a generic endpoint
    /// </summary>
    public interface IGenericEndpointTypeInfo : IEnumerable<IEndpointRouteValueMetadataProvider>
    {
        /// <summary>
        /// Type parameters of Generic endpoint
        /// </summary>
        Type[] TypeParameters { get; }

    }
}
