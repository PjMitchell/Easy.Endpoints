using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Easy.Endpoints
{
    /// <summary>
    /// Easy.Endpoints Extensions for IServiceCollection
    /// </summary>
    public static class EndpointServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Easy endpoints with default options and all Endpoints from calling assembly
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static EasyEndpointBuilder AddEasyEndpoints(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            return AddEmptyEasyEndpoints(services).WithEndpointsFromAssembly(assembly);

        }

        /// <summary>
        /// Adds Easy endpoints with default options and all Endpoints from calling assembly
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionBuilderActions">Declaration of how to build the endpoint options</param>
        [Obsolete("Use EasyEndpointBuilder to modify options instead")]
        public static EasyEndpointBuilder AddEasyEndpoints(this IServiceCollection services, Action<EndpointOptionBuilders> optionBuilderActions)
        {
            var assembly = Assembly.GetCallingAssembly();
            var optionBuilder = new EndpointOptionBuilders();
            optionBuilderActions(optionBuilder);
            return AddEmptyEasyEndpoints(services, optionBuilder.BuildOption()).WithEndpointsFromAssembly(assembly);
        }

        /// <summary>
        /// Adds Easy endpoints
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="manifestBuilderActions">Declaration of how to build the endpoint manifest</param>
        [Obsolete("Use AddEmptyEasyEndpoints instead")]
        public static EasyEndpointBuilder AddEasyEndpoints(this IServiceCollection services, Action<EndpointManifestBuilder> manifestBuilderActions)
        {
            var result = AddEmptyEasyEndpoints(services);
            manifestBuilderActions(new EndpointManifestBuilder(result));
            return result;
        }

        /// <summary>
        /// Adds Easy endpoints with no Endpoints loaded
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static EasyEndpointBuilder AddEmptyEasyEndpoints(this IServiceCollection services)
        {
            return AddEmptyEasyEndpoints(services, new EndpointOptions());
        }

        private static EasyEndpointBuilder AddEmptyEasyEndpoints(this IServiceCollection services, EndpointOptions options)
        {
            services.AddTransient(ManifestHelper.BuidManifest);
            services.AddSingleton(options);
            services.AddTransient<IEasyEndpointCompositeMetadataDetailsProvider, EasyEndpointCompositeMetadataDetailsProvider>();
            services.AddTransient<IApiDescriptionGroupCollectionProvider, EasyEndpointApiDescriptionGroupCollectionProvider>();
            var builderOptions = new EasyEndpointBuilderOptions();
            services.AddSingleton(builderOptions);

            services.AddTransient<IMalformedRequestExceptionHandler, DefaultIMalformedRequestExceptionHandler>();
            var builder = new EasyEndpointBuilder(services, builderOptions, options);
            AddDefaultParsers(builder);
            return builder;
        }

        private static void AddDefaultParsers(EasyEndpointBuilder endpointBuilder)
        {
            endpointBuilder.WithParser<DefaultParsers.ByteParser>();
            endpointBuilder.WithParser<DefaultParsers.UShortParser>();
            endpointBuilder.WithParser<DefaultParsers.ShortParser>();
            endpointBuilder.WithParser<DefaultParsers.UIntParser>();
            endpointBuilder.WithParser<DefaultParsers.IntParser>();
            endpointBuilder.WithParser<DefaultParsers.ULongParser>();
            endpointBuilder.WithParser<DefaultParsers.LongParser>();
            endpointBuilder.WithParser<DefaultParsers.FloatParser>();
            endpointBuilder.WithParser<DefaultParsers.DoubleParser>();
            endpointBuilder.WithParser<DefaultParsers.DecimalParser>();
            endpointBuilder.WithParser<DefaultParsers.BoolParser>();
            endpointBuilder.WithParser<DefaultParsers.DateTimeParser>();
            endpointBuilder.WithParser<DefaultParsers.DateTimeOffsetParser>();
            endpointBuilder.WithParser<DefaultParsers.DateOnlyParser>();
            endpointBuilder.WithParser<DefaultParsers.TimeOnlyParser>();
            endpointBuilder.WithParser<DefaultParsers.GuidParser>();
        }

    }
}
