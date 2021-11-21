using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class EndpointRequestHandlerFactoryBuilder
    {
        public static EndpointRequestHandlerDeclaration BuildFactoryForEndpoint(TypeInfo endpointType,DeclaredRouteInfo routeInfo, EndpointOptions options)
        {
            var handleMethods = endpointType.DeclaredMethods.Where(m => m.IsPublic && (m.Name is "Handle" or "HandleAsync")).ToArray();
            if (handleMethods.Length != 1 || handleMethods[0] is null)
                throw new InvalidEndpointSetupException($"{endpointType.Name} does not contain a single public method named Handle or HandleAsync");
            
            var handleMethod = handleMethods[0];
            var parameterInfo = BuildEndpointParameterInfo(handleMethod,routeInfo, options);
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            if (handleMethod.ReturnType == typeof(void))
                return BuildNoContentResponse(new VoidEndpointMethodExecutor(endpointType, handleMethod), parameterInfo);
            if (handleMethod.ReturnType == typeof(Task))
                return BuildNoContentResponse(new TaskEndpointMethodExecutor(endpointType, handleMethod), parameterInfo);
            if (handleMethod.ReturnType == typeof(ValueTask))
                return BuildNoContentResponse(new ValueTaskEndpointMethodExecutor(endpointType, handleMethod), parameterInfo);


            var (endpointMethodExecutor, returnType) = GetMethodExcutorAndReturnTypeFor(endpointType, handleMethod);

            if (typeof(IEndpointResult).IsAssignableFrom(returnType))
                return BuildForEndpointResultResponse(endpointMethodExecutor, parameterInfo, options);

            if (returnType == typeof(string))
                return BuildForStringResponse(endpointMethodExecutor, parameterInfo, returnType, options);
            
            return BuildForObjectResponse(endpointMethodExecutor, parameterInfo, returnType, options);
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
        private static EndpointRequestHandlerDeclaration BuildNoContentResponse(NoResultEndpointMethodExecutor voidEndpointMethodExecutor, EndpointParameterInfo[] parameterInfo)
        {
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            return  new EndpointRequestHandlerDeclaration(
                (ctx) => new NoContentRequestHandler(voidEndpointMethodExecutor, ctx, parameterArrayFactory),
                parameterInfo,
                typeof(void)
            );
        }

        private static EndpointRequestHandlerDeclaration BuildForObjectResponse(ObjectEndpointMethodExecutor objectEndpointMethodExecutor, EndpointParameterInfo[] parameterInfo,Type returnType, EndpointOptions options)
        {
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            return new EndpointRequestHandlerDeclaration(
                (ctx) => new ObjectRequestHandler(objectEndpointMethodExecutor, ctx, parameterArrayFactory, options),
                parameterInfo,
                returnType
            );
        }

        private static EndpointRequestHandlerDeclaration BuildForStringResponse(ObjectEndpointMethodExecutor objectEndpointMethodExecutor, EndpointParameterInfo[] parameterInfo, Type returnType, EndpointOptions options)
        {
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            return new EndpointRequestHandlerDeclaration(
                (ctx) => new StringRequestHandler(objectEndpointMethodExecutor, ctx, parameterArrayFactory, options),
                parameterInfo,
                returnType
            );
        }

        private static EndpointRequestHandlerDeclaration BuildForEndpointResultResponse(ObjectEndpointMethodExecutor objectEndpointMethodExecutor, EndpointParameterInfo[] parameterInfo, EndpointOptions options)
        {
            var parameterArrayFactory = BuildParameterArrayFactory(parameterInfo);
            return new EndpointRequestHandlerDeclaration(
                (ctx) => new EndpointResultRequestHandler(objectEndpointMethodExecutor, ctx, parameterArrayFactory, options),
                parameterInfo
            );
        }

        private static ParameterArrayFactory BuildParameterArrayFactory(EndpointParameterInfo[] parameter)
        {
            return async (HttpContext ctx) =>
            {
                object?[] results = new object?[parameter.Length];
                for (int i = 0; i < parameter.Length; i++)
                {
                    results[i] = await parameter[i].ParameterFactory(ctx);
                }
                return results;
            };
        }

        private static EndpointParameterInfo[] BuildEndpointParameterInfo(MethodInfo methodInfo, DeclaredRouteInfo declaredRouteInfo, EndpointOptions options)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters is null)
                return Array.Empty<EndpointParameterInfo>();
            EndpointParameterInfo[] results = new EndpointParameterInfo[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                results[i] = EndpointParameterInfoFactory.BuildEndpointParameterInfo(parameters[i],declaredRouteInfo.Parameters, options);
            }
            return results;
        }      

    }

    internal delegate ValueTask<object?[]> ParameterArrayFactory(HttpContext httpContext);
    internal delegate Task<object?> ObjectMethodCaller(object callee, object?[] parameters);

}
