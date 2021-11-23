using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Easy.Endpoints
{
    internal abstract class EndpointMethodExecutor
    {
        public readonly Type EndpointType;

        protected EndpointMethodExecutor(Type endpointType)
        {
            EndpointType = endpointType;
        }

        protected static (ParameterExpression parametersParameter, List<Expression> parameters) SetupHandlerParameters(MethodInfo handleMethod)
        {
            var parametersParameter = Expression.Parameter(typeof(object?[]), "parameters");
            var paramInfos = handleMethod.GetParameters();
            var parameters = new List<Expression>(paramInfos.Length);
            for (int i = 0; i < paramInfos.Length; i++)
            {
                var paramInfo = paramInfos[i];
                var valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(valueObj, paramInfo.ParameterType);
                parameters.Add(valueCast);
            }

            return (parametersParameter, parameters);
        }

        protected static (ParameterExpression targetParameter, UnaryExpression? endpointCast) SetupHandler(Type endpointType)
        {
            var targetParameter = Expression.Parameter(typeof(object), "target");
            var endpointCast = Expression.Convert(targetParameter, endpointType);
            return (targetParameter, endpointCast);
        }
    }

}
