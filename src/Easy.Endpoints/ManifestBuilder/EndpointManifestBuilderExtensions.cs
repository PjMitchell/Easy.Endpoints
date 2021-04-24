using System;
using System.Reflection;

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
        public static EndpointManifestBuilder AddForEndpoint<TEndpoint>(this EndpointManifestBuilder builder) where TEndpoint: IEndpoint => AddForEndpoint(builder, typeof(TEndpoint));

        /// <summary>
        /// Add Endpoints For Implementation of IEndpoint;
        /// </summary>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <param name="endpoint">Type of IEndpoint to be added</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpoint(this EndpointManifestBuilder builder, Type endpoint) => AddForEndpoint(builder, endpoint.GetTypeInfo());
        
        /// <summary>
        /// Add Endpoints For Implementation of IEndpoint;
        /// </summary>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <param name="endpoint">TypeInfo of IEndpoint to be added</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpoint(this EndpointManifestBuilder builder, TypeInfo endpoint)
        {
            var info = EndpointInfoFactory.BuildInfoForEndpoint(endpoint, builder.Options);
            builder.AddEndpoint(info);
            return builder;
        }

        /// <summary>
        /// Add Endpoints For Implementation of IEndpointHandler;
        /// </summary>
        /// <typeparam name="TendpointHandler">Type of IEndpointHandler to be added</typeparam>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpointHandler<TendpointHandler>(this EndpointManifestBuilder builder) where TendpointHandler : IEndpointHandler => AddForEndpointHandler(builder, typeof(TendpointHandler));

        /// <summary>
        /// Add Endpoints For Implementation of IEndpointHandler;
        /// </summary>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <param name="handler">Type of IEndpointHandler to be added</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpointHandler(this EndpointManifestBuilder builder, Type handler) => AddForEndpointHandler(builder, handler.GetTypeInfo());

        /// <summary>
        /// Add Endpoints For Implementation of IEndpointHandler;
        /// </summary>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <param name="handler">TypeInfo of IEndpointHandler to be added</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpointHandler(this EndpointManifestBuilder builder, TypeInfo handler)
        {
            var endpoint = GetEndpointForHandler(builder, handler);            
            var info = EndpointInfoFactory.BuildInfoForHandler(handler, endpoint.GetTypeInfo(), builder.Options);
            builder.AddEndpoint(info);
            return builder;
        }

        private static Type GetEndpointForHandler(EndpointManifestBuilder builder, TypeInfo handler)
        {
            foreach(var declaration in builder.Options.EndpointForHandlerDeclarations)
            {
                var endpoint = declaration.GetEndpointForHandler(handler);
                if (endpoint is not null)
                    return endpoint;
            }
            throw new InvalidEndpointSetupException($"Could not find Endpoint for Handler {handler.FullName}");
        }

    }
}
