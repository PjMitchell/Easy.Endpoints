using Microsoft.AspNetCore.Mvc.Filters;

namespace Easy.Endpoints
{
    /// <summary>
    /// Providers Endpoint route values information
    /// </summary>
    public interface IEndpointRouteValueMetadataProvider : IFilterMetadata
    {
        /// <summary>
        /// Get Endpoint Route Key
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Gets Endpoint Route Value
        /// </summary>
        string Value { get; }
    }
}
