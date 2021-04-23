using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <summary>
    /// Builds Endpoint manifest
    /// </summary>
    public class EndpointManifestBuilder
    {
        private readonly List<EndpointInfo> source;
        /// <summary>
        /// Option for endpoints
        /// </summary>
        public EndpointOptions Options { get; }

        /// <summary>
        /// Constructs new instance of EndpointManifestBuilder 
        /// </summary>
        /// <param name="options">EndpointOptions options for endpoints</param>
        public EndpointManifestBuilder(EndpointOptions options)
        {
            Options = options;
            source = new List<EndpointInfo>();
        }

        /// <summary>
        /// Adds a new endpoint to the manifest
        /// </summary>
        /// <param name="endpoint">Endpoint to be added to manifest</param>
        public void AddEndpoint(EndpointInfo endpoint)
        {
            source.Add(endpoint);
        }

        /// <summary>
        /// Builds Endpoint manifest
        /// </summary>
        /// <returns>New Endpoint manifest</returns>
        public IEndpointManifest Build()
        {
            return new EndpointManifest(source);
        }
    }
}
