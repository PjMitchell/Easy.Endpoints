using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class VoidEndpointMethodExecutor : NoResultEndpointMethodExecutor
    {
        public readonly Type ObjectReturnType;
        private readonly VoidEndpointHandleExcutor executor;

        public VoidEndpointMethodExecutor(Type endpointType, MethodInfo handleMethod) : base(endpointType)
        {
            ObjectReturnType = handleMethod.ReturnType;
            executor = BuildHandler(endpointType, handleMethod);
        }

        public override Task Execute(object instance, object?[] parameters)
        {
            executor(instance, parameters);
            return Task.CompletedTask;
        }

        private static VoidEndpointHandleExcutor BuildHandler(Type executorType, MethodInfo handleMethod)
        {
            var (targetParameter, instanceCast) = SetupHandler(executorType);
            var (parametersParameter, parameters) = SetupHandlerParameters(handleMethod);
            var methodCall = Expression.Call(instanceCast, handleMethod, parameters);
            var lambda = Expression.Lambda<VoidEndpointHandleExcutor>(methodCall, targetParameter, parametersParameter);
            return lambda.Compile();

        }

        public delegate void VoidEndpointHandleExcutor(object endpoint, object?[] context);
    }
}
