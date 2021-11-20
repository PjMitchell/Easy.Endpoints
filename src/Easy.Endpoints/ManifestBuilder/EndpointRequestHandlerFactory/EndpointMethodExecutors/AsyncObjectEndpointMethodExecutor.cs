using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class AsyncObjectEndpointMethodExecutor : ObjectEndpointMethodExecutor
    {
        public override Type ObjectReturnType { get; }
        private ObjectEndpointHandleExcutor executor;

        public AsyncObjectEndpointMethodExecutor(Type endpointType, MethodInfo handleMethod) : base(endpointType)
        {
            var objectReturnType = handleMethod.ReturnType.GenericTypeArguments[0];
            ObjectReturnType = objectReturnType;
            executor = BuildHandler(endpointType, handleMethod, objectReturnType);
        }

        public override Task<object> Execute(object instance, object?[] parameters)
        {
            return executor(instance, parameters);
        }

        private static ObjectEndpointHandleExcutor BuildHandler(Type executorType, MethodInfo handleMethod, Type objectReturnType)
        {
            var (targetParameter, instanceCast) = SetupHandler(executorType);
            var (parametersParameter, parameters) = SetupHandlerParameters(handleMethod);
            var methodCall = Expression.Call(instanceCast, handleMethod, parameters);
            var caster = typeof(TaskCaster<>).MakeGenericType(objectReturnType).GetMethod("Cast");
            var castMethodCall = Expression.Call(caster!,methodCall);
            var lambda = Expression.Lambda<ObjectEndpointHandleExcutor>(castMethodCall, targetParameter, parametersParameter);
            return lambda.Compile();

        }

        public delegate Task<object> ObjectEndpointHandleExcutor(object endpoint, object?[] context);

        public static async Task<object?> Cast<T>(Task<T> source) => (object?)await source.ConfigureAwait(false);
    }


}
