using System;
using System.Linq;
using System.Reflection;

namespace Easy.Endpoints
{
    /// <summary>
    /// Endpoints for JsonEndpointHandlers
    /// </summary>
    public class JsonEndpointForHandlerDeclarations : IEndpointForHandlerDeclaration
    {
        ///<inheritdoc cref="IEndpointForHandlerDeclaration"/>
        public Type? GetEndpointForHandler(TypeInfo handlerTypeInfo)
        {
            var jsonBodyHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => r.GenericTypeArguments.Length == 1 && r == typeof(IJsonBodyEndpointHandler<>).MakeGenericType(r.GenericTypeArguments[0]));
            if (jsonBodyHandler is not null)
                return typeof(JsonBodyEndpoint<,>).MakeGenericType(handlerTypeInfo.AsType(), jsonBodyHandler.GenericTypeArguments[0]);

            var jsonResponseHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => r.GenericTypeArguments.Length == 1 && r == typeof(IJsonResponseEndpointHandler<>).MakeGenericType(r.GenericTypeArguments[0]));
            if (jsonResponseHandler is not null)
                return typeof(JsonResponseEndpoint<,>).MakeGenericType(handlerTypeInfo.AsType(), jsonResponseHandler.GenericTypeArguments[0]);

            var jsonHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => r.GenericTypeArguments.Length == 2 && r == typeof(IJsonEndpointHandler<,>).MakeGenericType(r.GenericTypeArguments[0], r.GenericTypeArguments[1]));
            if (jsonHandler is not null)
                return typeof(JsonEndpoint<,,>).MakeGenericType(handlerTypeInfo.AsType(), jsonHandler.GenericTypeArguments[0], jsonHandler.GenericTypeArguments[1]);
            return null;
        }
    }
}
