namespace Easy.Endpoints
{
    /// <summary>
    /// Providers Endpoint route values information
    /// </summary>
    public class EndpointRouteValueMetadata : IEndpointRouteValueMetadataProvider
    {
        /// <summary>
        /// Creates new instance of EndpointRouteValueMetadata
        /// </summary>
        /// <param name="key">Key for route value</param>
        /// <param name="value">Value of route value</param>
        public EndpointRouteValueMetadata(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <inheritdoc cref="IEndpointRouteValueMetadataProvider.Key"/>
        public string Key { get; }

        /// <inheritdoc cref="IEndpointRouteValueMetadataProvider.Value"/>
        public string Value { get; }
    }
}
