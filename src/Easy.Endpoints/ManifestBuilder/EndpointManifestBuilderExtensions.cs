using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

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
        public static EndpointManifestBuilder AddForEndpoint<TEndpoint>(this EndpointManifestBuilder builder) where TEndpoint : IEndpoint => AddForEndpoint(builder, typeof(TEndpoint).GetTypeInfo());


        /// <summary>
        /// Add Endpoints For Implementation of IEndpoint;
        /// </summary>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <param name="endpoint">Type of IEndpoint to be added</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpoint(this EndpointManifestBuilder builder, Type endpoint) => AddForEndpoint(builder, (Type)endpoint.GetTypeInfo());


        /// <summary>
        /// Add Endpoints For Implementation of IEndpoint;
        /// </summary>
        /// <param name="builder">The current instance of the manifest builder</param>
        /// <param name="endpoint">TypeInfo of IEndpoint to be added</param>
        /// <returns>Same instance of the manifest builder</returns>
        public static EndpointManifestBuilder AddForEndpoint(this EndpointManifestBuilder builder, TypeInfo endpoint)
        {
            if (!endpoint.IsAssignableTo(typeof(IEndpoint)))
                throw new InvalidEndpointSetupException($"Could not assign {endpoint.FullName} to {nameof(IEndpoint)}");
            if (endpoint.IsGenericType)
            {
                foreach (var info in GetGenericEndpointInfo(endpoint).Where(i => i.TypeParameters.Length == endpoint.GenericTypeParameters.Length))
                {
                    var endpointType = endpoint.MakeGenericType(info.TypeParameters).GetTypeInfo();
                    builder.AddEndpoint(EndpointInfoFactory.BuildInfoForEndpoint(endpointType, builder.Options, info.ToArray()));
                }
            }
            else
            {
                builder.AddEndpoint(EndpointInfoFactory.BuildInfoForEndpoint(endpoint, builder.Options, Array.Empty<object>()));
            }
            return builder;
        }

        private static IEnumerable<IGenericEndpointTypeInfo> GetGenericEndpointInfo(TypeInfo handler)
        {
            foreach(var attribute in handler.GetCustomAttributes())
            {
                if (attribute is IGenericEndpointTypeInfo info)
                {
                    yield return info;
                }
                else if (attribute is IGenericEndpointTypeInfoProvider infoProvider)
                {
                    foreach (var item in infoProvider.GetGenericEndpointTypeInfo())
                        yield return item;
                }
            }
        }
    }
}
