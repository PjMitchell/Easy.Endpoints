using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Easy.Endpoints
{
    public static class EndpointServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Request endpoints with default options and all Endpoints from calling assembly
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        public static void AddRequestEndpoints(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddRequestEndpoints(services, NullOptionModifications, b => b.AddFromAsembly(assembly));
        }

        /// <summary>
        /// Adds Request endpoints with default options and all Endpoints from calling assembly
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionBuilderActions">Declaration of how to build the endpoint options</param>
        public static void AddRequestEndpoints(this IServiceCollection services, Action<EndpointOptionBuilders> optionBuilderActions)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddRequestEndpoints(services, optionBuilderActions, b => b.AddFromAsembly(assembly));
        }

        /// <summary>
        /// Adds Request endpoints
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="manifestBuilderActions">Declaration of how to build the endpoint manifest</param>
        public static void AddRequestEndpoints(this IServiceCollection services, Action<EndpointManifestBuilder> manifestBuilderActions)
        {
            AddRequestEndpoints(services, NullOptionModifications, manifestBuilderActions);
        }

        /// <summary>
        /// Adds Request endpoints
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <param name="optionBuilderActions">Declaration of how to build the endpoint options</param>
        /// <param name="manifestBuilderActions">Declaration of how to build the endpoint manifest</param>
        public static void AddRequestEndpoints(this IServiceCollection services, Action<EndpointOptionBuilders> optionBuilderActions, Action<EndpointManifestBuilder> manifestBuilderActions)
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
        }

        private static void NullOptionModifications(EndpointOptionBuilders options)
        {
            // Does nothing
        }
    }
}
