using System;
using System.Linq;
using System.Reflection;
using static Easy.Endpoints.GenericTypeHelper;

namespace Easy.Endpoints
{
    /// <summary>
    /// Endpoints for JsonEndpointHandlers
    /// </summary>
    public class JsonEndpointForHandlerDeclarations : IEndpointForHandlerDeclaration
    {
        ///<inheritdoc cref="IEndpointForHandlerDeclaration.GetEndpointForHandler(TypeInfo)"/>
        public Type? GetEndpointForHandler(TypeInfo handlerTypeInfo)
        {
            var jsonBodyHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonBodyEndpointHandler<>)));
            if (jsonBodyHandler is not null)
                return typeof(JsonBodyEndpoint<,>).MakeGenericType(handlerTypeInfo.AsType(), jsonBodyHandler.GenericTypeArguments[0]);

            var jsonResponseHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonResponseEndpointHandler<>)));
            if (jsonResponseHandler is not null)
                return typeof(JsonResponseEndpoint<,>).MakeGenericType(handlerTypeInfo.AsType(), jsonResponseHandler.GenericTypeArguments[0]);

            var urlParameterEndpointHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IUrlParametersEndpointHandler<>)));
            if (urlParameterEndpointHandler is not null)
                return typeof(UrlParametersEndpoint<,>).MakeGenericType(handlerTypeInfo.AsType(), urlParameterEndpointHandler.GenericTypeArguments[0]);

            var jsonHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonBodyAndResponseEndpointHandler<,>)));
            if (jsonHandler is not null)
                return typeof(JsonBodyAndResponseEndpoint<,,>).MakeGenericType(handlerTypeInfo.AsType(), jsonHandler.GenericTypeArguments[0], jsonHandler.GenericTypeArguments[1]);

            var jsonBodyWithUrlParameterHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonBodyWithUrlParametersEndpointHandler<,>)));
            if (jsonBodyWithUrlParameterHandler is not null)
                return typeof(JsonBodyWithUrlParametersEndpoint<,,>).MakeGenericType(handlerTypeInfo.AsType(), jsonBodyWithUrlParameterHandler.GenericTypeArguments[0], jsonBodyWithUrlParameterHandler.GenericTypeArguments[1]);

            var jsonResponseWithUrlParameterEndpointHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonResponseWithUrlParametersEndpointHandler<,>)));
            if (jsonResponseWithUrlParameterEndpointHandler is not null)
                return typeof(JsonResponseWithUrlParametersEndpoint<,,>).MakeGenericType(handlerTypeInfo.AsType(), jsonResponseWithUrlParameterEndpointHandler.GenericTypeArguments[0], jsonResponseWithUrlParameterEndpointHandler.GenericTypeArguments[1]);

            var jsonResponseAndBodyWithUrlParameterEndpointHandler = handlerTypeInfo.ImplementedInterfaces.SingleOrDefault(r => MatchExpectedGeneric(r, typeof(IJsonBodyAndResponseWithUrlParametersEndpointHandler<,,>)));
            if (jsonResponseAndBodyWithUrlParameterEndpointHandler is not null)
                return typeof(JsonBodyAndResponseWithUrlParametersEndpoint<,,,>).MakeGenericType(handlerTypeInfo.AsType(), jsonResponseAndBodyWithUrlParameterEndpointHandler.GenericTypeArguments[0], jsonResponseAndBodyWithUrlParameterEndpointHandler.GenericTypeArguments[1], jsonResponseAndBodyWithUrlParameterEndpointHandler.GenericTypeArguments[2]);
            return null;
        }
    }
}
