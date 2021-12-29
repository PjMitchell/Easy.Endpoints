using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class EndpointRequestHandlerFactoryBuilder
    {
        public static EndpointRequestHandlerDeclaration BuildFactoryForEndpoint(TypeInfo endpointType,DeclaredRouteInfo routeInfo, EndpointDeclarationFactoryOptions options)
        {
            var handleMethods = endpointType.DeclaredMethods.Where(m => m.IsPublic && (m.Name is "Handle" or "HandleAsync")).ToArray();
            if (handleMethods.Length != 1 || handleMethods[0] is null)
                throw new InvalidEndpointSetupException($"{endpointType.Name} does not contain a single public method named Handle or HandleAsync");
            
            var handleMethod = handleMethods[0];
            var parameterInfo = BuildEndpointParameterInfo(handleMethod,routeInfo, options);
            if (handleMethod.ReturnType == typeof(void))
                return BuildNoContentResponse(new VoidEndpointMethodExecutor(endpointType, handleMethod), parameterInfo);
            if (handleMethod.ReturnType == typeof(Task))
                return BuildNoContentResponse(new TaskEndpointMethodExecutor(endpointType, handleMethod), parameterInfo);
            if (handleMethod.ReturnType == typeof(ValueTask))
                return BuildNoContentResponse(new ValueTaskEndpointMethodExecutor(endpointType, handleMethod), parameterInfo);
            
            var (endpointMethodExecutor, returnType) = GetMethodExcutorAndReturnTypeFor(endpointType, handleMethod);

            if (typeof(IEndpointResult).IsAssignableFrom(returnType))
                return BuildForEndpointResultResponse(endpointMethodExecutor, parameterInfo);

            if (returnType == typeof(string))
                return BuildForStringResponse(endpointMethodExecutor, parameterInfo, returnType);
            
            return BuildForObjectResponse(endpointMethodExecutor, parameterInfo, returnType);
        }

        private static (ObjectEndpointMethodExecutor endpointMethodExecutor, Type returnType) GetMethodExcutorAndReturnTypeFor(Type endpointType, MethodInfo handleMethod)
        {
            if (handleMethod.ReturnType.IsGenericType && handleMethod.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return (new TaskObjectEndpointMethodExecutor(endpointType, handleMethod), handleMethod.ReturnType.GetGenericArguments()[0]);
            }

            if (handleMethod.ReturnType.IsGenericType && handleMethod.ReturnType.GetGenericTypeDefinition() == typeof(ValueTask<>))
            {
                return (new ValueTaskObjectEndpointMethodExecutor(endpointType, handleMethod), handleMethod.ReturnType.GetGenericArguments()[0]);
            }

            return (new SyncObjectEndpointMethodExecutor(endpointType, handleMethod), handleMethod.ReturnType);
        }
        private static EndpointRequestHandlerDeclaration BuildNoContentResponse(NoResultEndpointMethodExecutor voidEndpointMethodExecutor, EndpointHandlerParameterDeclaration[] parameterInfo)
        {
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            return  new EndpointRequestHandlerDeclaration(
                (ctx) => new NoContentRequestHandler(voidEndpointMethodExecutor, ctx, parameterArrayFactory),
                parameterInfo,
                typeof(void)
            );
        }

        private static EndpointRequestHandlerDeclaration BuildForObjectResponse(ObjectEndpointMethodExecutor objectEndpointMethodExecutor, EndpointHandlerParameterDeclaration[] parameterInfo,Type returnType)
        {
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            return new EndpointRequestHandlerDeclaration(
                (ctx) => new ObjectRequestHandler(objectEndpointMethodExecutor, ctx, parameterArrayFactory),
                parameterInfo,
                returnType
            );
        }

        private static EndpointRequestHandlerDeclaration BuildForStringResponse(ObjectEndpointMethodExecutor objectEndpointMethodExecutor, EndpointHandlerParameterDeclaration[] parameterInfo, Type returnType)
        {
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            return new EndpointRequestHandlerDeclaration(
                (ctx) => new StringRequestHandler(objectEndpointMethodExecutor, ctx, parameterArrayFactory),
                parameterInfo,
                returnType
            );
        }

        private static EndpointRequestHandlerDeclaration BuildForEndpointResultResponse(ObjectEndpointMethodExecutor objectEndpointMethodExecutor, EndpointHandlerParameterDeclaration[] parameterInfo)
        {
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            return new EndpointRequestHandlerDeclaration(
                (ctx) => new EndpointResultRequestHandler(objectEndpointMethodExecutor, ctx, parameterArrayFactory),
                parameterInfo
            );
        }

        private static ParameterArrayFactory BuildParameterArrayFactory(EndpointHandlerParameterDeclaration[] parameter)
        {
            return async (HttpContext ctx, EndpointOptions opts) =>
            {
                var modelErrorState = new BindingErrorCollection();
                object?[] results = new object?[parameter.Length];
                for (int i = 0; i < parameter.Length; i++)
                {
                    var value = await parameter[i].Factory(ctx, opts, modelErrorState);
                    results[i] = value.Result;
                }
                if(modelErrorState.HasErrors)
                    throw new MalformedRequestException(modelErrorState);
                return results;
            };
        }

        private static EndpointHandlerParameterDeclaration[] BuildEndpointParameterInfo(MethodInfo methodInfo, DeclaredRouteInfo declaredRouteInfo, EndpointDeclarationFactoryOptions options)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters is null)
                return Array.Empty<EndpointHandlerParameterDeclaration>();
            EndpointHandlerParameterDeclaration[] results = new EndpointHandlerParameterDeclaration[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                results[i] = EndpointParameterInfoFactory.BuildEndpointParameterDeclaration(parameters[i],declaredRouteInfo.Parameters, options);
            }
            return results;
        }      

    }

    internal delegate ValueTask<object?[]> ParameterArrayFactory(HttpContext httpContext, EndpointOptions opts);
    internal delegate Task<object?> ObjectMethodCaller(object callee, object?[] parameters);

}
