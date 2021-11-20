using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class SyncObjectEndpointMethodExecutor : ObjectEndpointMethodExecutor
    {
        public override Type ObjectReturnType { get; }
        private ObjectEndpointHandleExcutor executor;

        public SyncObjectEndpointMethodExecutor(Type endpointType, MethodInfo handleMethod) : base(endpointType)
        {
            ObjectReturnType = handleMethod.ReturnType;
            executor = BuildHandler(endpointType, handleMethod);
        }

        public override Task<object> Execute(object instance, object?[] parameters)
        {
            return Task.FromResult(executor(instance, parameters));
        }

        private static ObjectEndpointHandleExcutor BuildHandler(Type executorType, MethodInfo handleMethod)
        {
            var (targetParameter, instanceCast) = SetupHandler(executorType);
            var (parametersParameter, parameters) = SetupHandlerParameters(handleMethod);
            var methodCall = Expression.Call(instanceCast, handleMethod, parameters);

            var castMethodCall = Expression.Convert(methodCall, typeof(object));
            var lambda = Expression.Lambda<ObjectEndpointHandleExcutor>(castMethodCall, targetParameter, parametersParameter);
            return lambda.Compile();

        }
        public delegate object ObjectEndpointHandleExcutor(object endpoint, object?[] context);
    }
}
