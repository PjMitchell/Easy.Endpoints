using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class TaskEndpointMethodExecutor : NoResultEndpointMethodExecutor
    {
        private readonly TaskEndpointHandleExcutor executor;

        public TaskEndpointMethodExecutor(Type endpointType, MethodInfo handleMethod) : base(endpointType)
        {
            executor = BuildHandler(endpointType, handleMethod);
        }

        public override Task Execute(object instance, object?[] parameters) => executor(instance, parameters);

        private static TaskEndpointHandleExcutor BuildHandler(Type executorType, MethodInfo handleMethod)
        {
            var (targetParameter, instanceCast) = SetupHandler(executorType);
            var (parametersParameter, parameters) = SetupHandlerParameters(handleMethod);
            var methodCall = Expression.Call(instanceCast, handleMethod, parameters);
            var castMethodCall = Expression.Convert(methodCall, typeof(Task));
            var lambda = Expression.Lambda<TaskEndpointHandleExcutor>(castMethodCall, targetParameter, parametersParameter);
            return lambda.Compile();
        }

        public delegate Task TaskEndpointHandleExcutor(object endpoint, object?[] context);
    }
}
