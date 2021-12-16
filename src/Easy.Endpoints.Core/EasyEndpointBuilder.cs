using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Easy.Endpoints
{
    /// <summary>
    /// Builder for easy endpoint
    /// </summary>
    public class EasyEndpointBuilder
    {
        private readonly IServiceCollection serviceCollection;
        private readonly EasyEndpointBuilderOptions builderOptions;
        private readonly EndpointOptions endpointOptions;

        internal EasyEndpointBuilder(IServiceCollection serviceCollection, EasyEndpointBuilderOptions builderOptions,  EndpointOptions endpointOptions)
        {
            this.serviceCollection = serviceCollection;
            this.endpointOptions = endpointOptions;
            this.builderOptions = builderOptions;
        }

        /// <summary>
        /// Adds a new endpoint to the manifest
        /// </summary>
        /// <param name="endpoint">Endpoint to be added to manifest</param>
        public EasyEndpointBuilder WithEndpoint(Type endpoint)
        {
            
            if (endpoint.IsGenericTypeDefinition)
            {
                var typeInfo = endpoint.GetTypeInfo();
                foreach (var info in ManifestHelper.GetAllGenericEndpointInfo(endpoint).Where(i => i.TypeParameters.Length == typeInfo.GenericTypeParameters.Length))
                {
                    var endpointType = endpoint.MakeGenericType(info.TypeParameters);
                    WithEndpoint(endpointType);
                }
            }
            else
            {
                serviceCollection.AddTransient(endpoint);
                serviceCollection.AddTransient(typeof(IEndpoint), endpoint);
            }

            return this;
        }

        /// <summary>
        /// Adds IEndpointMetaDataDeclaration to builder
        /// </summary>
        /// <typeparam name="TEndpointMetaDataDeclaration">Type of IEndpointMetaDataDeclaration to add</typeparam>
        /// <returns>Updated instance of the option builder</returns>
        public EasyEndpointBuilder WithMetaDataDeclaration<TEndpointMetaDataDeclaration>() where TEndpointMetaDataDeclaration: class, IEndpointMetaDataDeclaration
        {
            serviceCollection.AddTransient<IEndpointMetaDataDeclaration, TEndpointMetaDataDeclaration>();
            return this;
        }

        /// <summary>
        /// Add Parser for parameter binding, for IParser T the last one added will be used for type T
        /// </summary>
        /// <param name="parser">Parser to add</param>
        /// <returns>Updated instance of the option builder</returns>
        public EasyEndpointBuilder WithParser(IParser parser)
        {
            serviceCollection.AddSingleton(typeof(IParser), parser);
            return this;
        }

        /// <summary>
        /// Add Parser for parameter binding, for IParser T the last one added will be used for type T
        /// </summary>
        /// <typeparam name="TParser">Type of parser to add</typeparam>
        /// <returns>Updated instance of the option builder</returns>
        public EasyEndpointBuilder WithParser<TParser>() where TParser : class, IParser
        {
            serviceCollection.AddTransient<IParser, TParser>();
            return this;
        }

        /// <summary>
        /// Defines Route Pattern when no route is defined for an endpoint
        /// </summary>
        /// <param name="routePattern">new route pattern for endpoint</param>
        /// <returns>Updated instance of the option builder</returns>
        public EasyEndpointBuilder WithRoutePattern(string routePattern)
        {
            builderOptions.RoutePattern = routePattern;
            return this;
        }

        /// <summary>
        /// Adds Json Serializer Options, if not defined default settings will be used
        /// </summary>
        /// <param name="jsonSerializerOptions">jsonSerializerOptions to be used by endpoint</param>
        /// <returns>Updated instance of the option builder</returns>
        public EasyEndpointBuilder WithJsonSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            endpointOptions.JsonSerializerOptions = jsonSerializerOptions;
            return this;
        }

        /// <summary>
        /// Modifies default JsonSerializerOptions for Endpoints
        /// </summary>
        /// <param name="jsonSerializerOptionsModification">Action that modifies JsonSerializerOptions</param>
        /// <returns>Updated instance of the option builder</returns>
        public EasyEndpointBuilder WithJsonSerializer(Action<JsonSerializerOptions> jsonSerializerOptionsModification)
        {
            jsonSerializerOptionsModification(endpointOptions.JsonSerializerOptions);
            return this;
        }

        /// <summary>
        /// Adds IFormatProvider, if not defined Invariant Culture will be used
        /// </summary>
        /// <param name="formatProvider">Format Provider to be used by endpoint</param>
        /// <returns>Updated instance of the option builder</returns>
        public EasyEndpointBuilder WithFormatProvider(IFormatProvider formatProvider)
        {
            endpointOptions.FormatProvider = formatProvider;
            return this;
        }
    }
}
