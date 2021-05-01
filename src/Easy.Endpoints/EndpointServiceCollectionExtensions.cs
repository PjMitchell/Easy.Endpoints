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
        /// Adds Request endpoints with default options and all Endpoints from calling assembly
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        [Obsolete("Use AddEasyEndpoints")]
        public static void AddRequestEndpoints(this IServiceCollection services) => AddEasyEndpoints(services);

        /// <summary>
        /// Adds Request endpoints with default options and all Endpoints from calling assembly
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionBuilderActions">Declaration of how to build the endpoint options</param>
        [Obsolete("Use AddEasyEndpoints")]
        public static void AddRequestEndpoints(this IServiceCollection services, Action<EndpointOptionBuilders> optionBuilderActions) => AddEasyEndpoints(services, optionBuilderActions);

        /// <summary>
        /// Adds Request endpoints
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="manifestBuilderActions">Declaration of how to build the endpoint manifest</param>
        [Obsolete("Use AddEasyEndpoints")]
        public static void AddRequestEndpoints(this IServiceCollection services, Action<EndpointManifestBuilder> manifestBuilderActions) => AddEasyEndpoints(services, manifestBuilderActions);

        /// <summary>
        /// Adds Request endpoints
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionBuilderActions">Declaration of how to build the endpoint options</param>
        /// <param name="manifestBuilderActions">Declaration of how to build the endpoint manifest</param>
        [Obsolete("Use AddEasyEndpoints")]
        public static void AddRequestEndpoints(this IServiceCollection services, Action<EndpointOptionBuilders> optionBuilderActions, Action<EndpointManifestBuilder> manifestBuilderActions) => AddEasyEndpoints(services, optionBuilderActions, manifestBuilderActions);

        /// <summary>
        /// Adds Easy endpoints with default options and all Endpoints from calling assembly
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static void AddEasyEndpoints(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddEasyEndpoints(services, NullOptionModifications, b => b.AddFromAssembly(assembly));
        }

        /// <summary>
        /// Adds Easy endpoints with default options and all Endpoints from calling assembly
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionBuilderActions">Declaration of how to build the endpoint options</param>
        public static void AddEasyEndpoints(this IServiceCollection services, Action<EndpointOptionBuilders> optionBuilderActions)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddEasyEndpoints(services, optionBuilderActions, b => b.AddFromAssembly(assembly));
        }

        /// <summary>
        /// Adds Easy endpoints
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="manifestBuilderActions">Declaration of how to build the endpoint manifest</param>
        public static void AddEasyEndpoints(this IServiceCollection services, Action<EndpointManifestBuilder> manifestBuilderActions)
        {
            AddEasyEndpoints(services, NullOptionModifications, manifestBuilderActions);
        }

        /// <summary>
        /// Adds Easy endpoints
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionBuilderActions">Declaration of how to build the endpoint options</param>
        /// <param name="manifestBuilderActions">Declaration of how to build the endpoint manifest</param>
        public static void AddEasyEndpoints(this IServiceCollection services, Action<EndpointOptionBuilders> optionBuilderActions, Action<EndpointManifestBuilder> manifestBuilderActions)
        {
            var optionBuilder = new EndpointOptionBuilders();
            optionBuilderActions(optionBuilder);
            var options = optionBuilder.BuildOption();
            var manifestBuilder = new EndpointManifestBuilder(options);
            manifestBuilderActions(manifestBuilder);

            var manifest = manifestBuilder.Build();
            foreach (var endpointInfo in manifest)
            {
                services.AddTransient(endpointInfo.Type);
                if (endpointInfo.Handler is not null)
                    services.AddTransient(endpointInfo.Handler);
            }
            services.AddSingleton(manifest);
            services.AddSingleton(options);
            services.AddTransient<IEasyEndpointCompositeMetadataDetailsProvider, EasyEndpointCompositeMetadataDetailsProvider>();
            services.AddTransient<IApiDescriptionGroupCollectionProvider, EasyEndpointApiDescriptionGroupCollectionProvider>();
            services.AddScoped<EndpointContextAccessor>();
            services.AddTransient<IEndpointContextAccessor>(s => s.GetRequiredService<EndpointContextAccessor>());
            services.AddTransient<IIntIdRouteParser, IntIdRouteParser>();
            services.AddTransient<IGuidIdRouteParser, GuidIdRouteParser>();
            services.AddTransient<IStringIdRouteParser, StringIdRouteParser>();
        }

        private static void NullOptionModifications(EndpointOptionBuilders options)
        {
            // Does nothing
        }
    }
}
