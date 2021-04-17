using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Easy.Endpoints
{
    public static class EndpointServiceCollectionExtensions
    {
        public static void AddRequestEndpoints(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddRequestEndpoints(services, b => b.AddFromAsembly(assembly));
        }

        public static void AddRequestEndpoints(this IServiceCollection services, Action<EndpointManifestBuilder> actions)
        {
            var manifestBuilder = new EndpointManifestBuilder();
            actions(manifestBuilder);

            var manifest = manifestBuilder.Build();
            foreach (var endpointInfo in manifest)
            {
                services.AddTransient(endpointInfo.Type);
                if (endpointInfo.Handler is not null)
                    services.AddTransient(endpointInfo.Handler);
            }
            services.AddSingleton(manifest);

            services.AddTransient<IEasyEndpointCompositeMetadataDetailsProvider, EasyEndpointCompositeMetadataDetailsProvider>();
            services.AddTransient<IApiDescriptionGroupCollectionProvider, EasyEndpointApiDescriptionGroupCollectionProvider>();
            services.AddScoped<EndpointContextAccessor>();
            services.AddTransient<IEndpointContextAccessor>(s => s.GetRequiredService<EndpointContextAccessor>());
            services.AddTransient<IIntIdRouteParser, IntIdRouteParser>();
        }
    }
}
