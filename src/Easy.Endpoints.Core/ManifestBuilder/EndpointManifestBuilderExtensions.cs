using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Extensions to the manifest builder to aid with adding endpoints
    /// </summary>
    public static class EndpointManifestBuilderExtensions
    {
        /// <summary>
        /// Add Endpoints For Implementation of IEndpoint;
        /// </summary>
        /// <typeparam name="TEndpoint">Type of IEndpoint to be added</typeparam>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpoint<TEndpoint>(this EndpointManifestBuilder builder) where TEndpoint : IEndpoint => AddForEndpoint(builder, typeof(TEndpoint));

        /// <summary>
        /// Add Endpoints For Implementation of IEndpoint;
        /// </summary>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <param name="endpoint">Type of IEndpoint to be added</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpoint(this EndpointManifestBuilder builder, Type endpoint)
        {
            builder.AddEndpoint(endpoint);
            return builder;
        }
    }
}
