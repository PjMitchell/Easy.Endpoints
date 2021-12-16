using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Builds Endpoint manifest
    /// </summary>
    public class EndpointManifestBuilder
    {
        private readonly EasyEndpointBuilder builder;

        internal EndpointManifestBuilder(EasyEndpointBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Adds a new endpoint to the manifest
        /// </summary>
        /// <param name="endpoint">Endpoint to be added to manifest</param>
        public void AddEndpoint(Type endpoint)
        {
            builder.WithEndpoint(endpoint);
        }
    }
}
