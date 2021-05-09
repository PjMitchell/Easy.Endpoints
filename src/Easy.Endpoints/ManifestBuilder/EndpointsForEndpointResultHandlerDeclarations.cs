using System;
using System.Linq;
using System.Reflection;
using static Easy.Endpoints.GenericTypeHelper;

namespace Easy.Endpoints
{
    /// <summary>
    /// Endpoints for EndpointResultHandlers
    /// </summary>
    public class EndpointsForEndpointResultHandlerDeclarations : IEndpointForHandlerDeclaration
    {
        ///<inheritdoc cref="IEndpointForHandlerDeclaration.GetEndpointForHandler(TypeInfo)"/>
        public Type? GetEndpointForHandler(TypeInfo handlerTypeInfo)
        {
            var endpointResultHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => r == typeof(IEndpointResultHandler));
            if (endpointResultHandler is not null)
                return typeof(EndpointResultHandlerEndpoint<>).MakeGenericType(handlerTypeInfo.AsType());

            var jsonBodyHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonBodyEndpointResultHandler<>)));
            if (jsonBodyHandler is not null)
                return typeof(JsonBodyEndpointResultHandlerEndpoint<,>).MakeGenericType(handlerTypeInfo.AsType(), jsonBodyHandler.GenericTypeArguments[0]);

            var jsonResponseHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IUrlParametersEndpointResultHandler<>)));
            if (jsonResponseHandler is not null)
                return typeof(UrlParametersEndpointResultHandlerEndpoint<,>).MakeGenericType(handlerTypeInfo.AsType(), jsonResponseHandler.GenericTypeArguments[0]);


            var jsonHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonBodyWithUrlParametersEndpointResultHandler<,>)));
            if (jsonHandler is not null)
                return typeof(JsonBodyWithUrlParametersEndpointResultHandlerEndpoint<,,>).MakeGenericType(handlerTypeInfo.AsType(), jsonHandler.GenericTypeArguments[0], jsonHandler.GenericTypeArguments[1]);

            return null;
        }
    }
}
