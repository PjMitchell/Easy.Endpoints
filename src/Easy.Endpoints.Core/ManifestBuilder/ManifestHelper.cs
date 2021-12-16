using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Endpoints
{
    internal static class ManifestHelper
    {

        public static IEndpointManifest BuidManifest(IServiceProvider serviceProvider)
        {
            var info = new List<EndpointDeclaration>();
            var options = serviceProvider.GetRequiredService<EasyEndpointBuilderOptions>();
            var endpointMetaDataDeclarations = serviceProvider.GetServices<IEndpointMetaDataDeclaration>();
            var parsers = new DefaultParserCollection(serviceProvider.GetServices<IParser>());
            var manifestOptions = new EndpointDeclarationFactoryOptions(options.RoutePattern, endpointMetaDataDeclarations, parsers);
            foreach (var endpoint in serviceProvider.GetServices<IEndpoint>())
            {
                var endpointOfType = endpoint.GetType();
                info.Add(EndpointDeclarationFactory.BuildDeclarationForEndpoint(endpointOfType.GetTypeInfo(), manifestOptions, GetGenericEndpointInfoForType(endpointOfType).ToArray()));
            }

            return new EndpointManifest(info);
        }

        private static IEnumerable<IEndpointRouteValueMetadataProvider> GetGenericEndpointInfoForType(Type endpoint)
        {
            if (!endpoint.IsGenericType)
                return Enumerable.Empty<IEndpointRouteValueMetadataProvider>();
            var genericParameters = endpoint.GetGenericArguments();
            return GetAllGenericEndpointInfo(endpoint).Where(t => TypesMatch(t.TypeParameters, genericParameters)).SelectMany(s => s);
        }

        public static IEnumerable<IGenericEndpointTypeInfo> GetAllGenericEndpointInfo(Type endpoint)
        {
            foreach (var attribute in endpoint.GetCustomAttributes())
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

        private static bool TypesMatch(Type[] a, Type[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
    }
}
