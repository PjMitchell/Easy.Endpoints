using System;
using System.Linq;
using System.Reflection;

namespace Easy.Endpoints
{
    public static class EndpointFromAssemblyExtensions
    {
        public static EndpointManifestBuilder AddFromAsembly(this EndpointManifestBuilder builder, Assembly assembly)
        {
            foreach (var endpoint in assembly.DefinedTypes.Where(IsRequestEndpoint))
            {
                var info = EndpointInfoFactory.BuildInfoForEndpoint(endpoint);
                builder.AddEndpoint(info);
            }

            foreach (var handler in assembly.DefinedTypes.Where(IsRequestHandler))
            {
                var endpoint = GetEndpointForHandler(handler);
                if (endpoint is null)
                    continue;
                var info = EndpointInfoFactory.BuildInfoForHandler(handler, endpoint.GetTypeInfo());
                builder.AddEndpoint(info);
            }

            return builder;
        }

        private static Type? GetEndpointForHandler(TypeInfo typeInfo)
        {
            var jsonBodyHandler = typeInfo.ImplementedInterfaces.SingleOrDefault(r => r.GenericTypeArguments.Length == 1 && r == typeof(IJsonBodyEndpointHandler<>).MakeGenericType(r.GenericTypeArguments[0]));
            if (jsonBodyHandler is not null)
                return typeof(JsonBodyEndpoint<,>).MakeGenericType(typeInfo.AsType(), jsonBodyHandler.GenericTypeArguments[0]);

            var jsonResponseHandler = typeInfo.ImplementedInterfaces.SingleOrDefault(r => r.GenericTypeArguments.Length == 1 && r == typeof(IJsonResponseEndpointHandler<>).MakeGenericType(r.GenericTypeArguments[0]));
            if (jsonResponseHandler is not null)
                return typeof(JsonResponseEndpoint<,>).MakeGenericType(typeInfo.AsType(), jsonResponseHandler.GenericTypeArguments[0]);

            var jsonHandler = typeInfo.ImplementedInterfaces.SingleOrDefault(r => r.GenericTypeArguments.Length == 2 && r == typeof(IJsonEndpointHandler<,>).MakeGenericType(r.GenericTypeArguments[0], r.GenericTypeArguments[1]));
            if (jsonHandler is not null)
                return typeof(JsonEndpoint<,,>).MakeGenericType(typeInfo.AsType(), jsonHandler.GenericTypeArguments[0], jsonHandler.GenericTypeArguments[1]);
            return null;
        }

        private static bool IsRequestEndpoint(TypeInfo t) => t.IsAssignableTo(typeof(IEndpoint));
        private static bool IsRequestHandler(TypeInfo t) => t.IsAssignableTo(typeof(IEndpointHandler));
    }
}
