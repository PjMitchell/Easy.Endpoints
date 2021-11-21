using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal abstract class AsyncObjectEndpointMethodExecutor : ObjectEndpointMethodExecutor
    {
        public AsyncObjectEndpointMethodExecutor(Type endpointType) : base(endpointType)
        {

        }

        protected static ObjectEndpointHandleExcutor BuildHandler(Type executorType, MethodInfo handleMethod, Type objectReturnType, GetCastExpression getCastExpression)
        {
            var (targetParameter, instanceCast) = SetupHandler(executorType);
            var (parametersParameter, parameters) = SetupHandlerParameters(handleMethod);
            var methodCall = Expression.Call(instanceCast, handleMethod, parameters);
            var castMethodCall = getCastExpression(objectReturnType, methodCall);
            var lambda = Expression.Lambda<ObjectEndpointHandleExcutor>(castMethodCall, targetParameter, parametersParameter);
            return lambda.Compile();

        }
        protected delegate MethodCallExpression GetCastExpression(Type objectReturnType, MethodCallExpression methodCall);
        public delegate Task<object> ObjectEndpointHandleExcutor(object endpoint, object?[] context);
    }

    internal class ValueTaskObjectEndpointMethodExecutor : AsyncObjectEndpointMethodExecutor
    {
        public override Type ObjectReturnType { get; }
        private ObjectEndpointHandleExcutor executor;

        public ValueTaskObjectEndpointMethodExecutor(Type endpointType, MethodInfo handleMethod) : base(endpointType)
        {
            var objectReturnType = handleMethod.ReturnType.GenericTypeArguments[0];
            ObjectReturnType = objectReturnType;
            executor = BuildHandler(endpointType, handleMethod, objectReturnType, BuildCastExpression);
        }

        public override Task<object> Execute(object instance, object?[] parameters)
        {
            return executor(instance, parameters);
        }

        private static MethodCallExpression BuildCastExpression(Type objectReturnType, MethodCallExpression methodCall)
        {
            var caster = typeof(ValueTaskCaster<>).MakeGenericType(objectReturnType).GetMethod("Cast");
            return Expression.Call(caster!, methodCall);
        }

    }

    internal class TaskObjectEndpointMethodExecutor : AsyncObjectEndpointMethodExecutor
    {
        public override Type ObjectReturnType { get; }
        private ObjectEndpointHandleExcutor executor;

        public TaskObjectEndpointMethodExecutor(Type endpointType, MethodInfo handleMethod) : base(endpointType)
        {
            var objectReturnType = handleMethod.ReturnType.GenericTypeArguments[0];
            ObjectReturnType = objectReturnType;
            executor = BuildHandler(endpointType, handleMethod, objectReturnType, BuildCastExpression);
        }

        public override Task<object> Execute(object instance, object?[] parameters)
        {
            return executor(instance, parameters);
        }

        private static MethodCallExpression BuildCastExpression(Type objectReturnType, MethodCallExpression methodCall)
        {
            var caster = typeof(TaskCaster<>).MakeGenericType(objectReturnType).GetMethod("Cast");
            return Expression.Call(caster!, methodCall);
        }
    }


}
