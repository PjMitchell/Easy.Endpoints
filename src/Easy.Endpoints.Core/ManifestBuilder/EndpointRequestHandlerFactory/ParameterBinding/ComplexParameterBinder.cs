using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Easy.Endpoints
{
    internal static class ComplexParameterBinder
    {
        private const byte none = 0;
        private const byte error = 2;

        private static readonly ParameterExpression ctxParameterExpr = Expression.Parameter(typeof(HttpContext), "ee_ctx");
        private static readonly ParameterExpression optionsParameterExpr = Expression.Parameter(typeof(EndpointOptions), "ee_options");
        private static readonly ParameterExpression errorCollectionParameterExpr = Expression.Parameter(typeof(IBindingErrorCollection), "ee_errorCollection");
        private static readonly MethodInfo  invokeMethod = typeof(SyncParameterFactory).GetMethod("Invoke")!;
        private static readonly ConstructorInfo parameterBindingResultCtor = typeof(ParameterBindingResult).GetConstructor(new[] { typeof(object), typeof(ParameterBindingIssues) })!;
        private static readonly PropertyInfo parameterBindingResultProperty = typeof(ParameterBindingResult).GetProperty(nameof(ParameterBindingResult.Result))!;
        private static readonly PropertyInfo parameterBindingStateProperty = typeof(ParameterBindingResult).GetProperty(nameof(ParameterBindingResult.State))!;
        
        public static (SyncParameterFactory factory, EndpointParameterDescriptor[] info) GetParameterDeclarationForClass(Type type, EndpointParameterSource source, EndpointDeclarationFactoryOptions options)
        {
            var constructor = type.GetConstructor(Array.Empty<Type>());
            if (constructor is null)
                throw new InvalidEndpointSetupException($"No parameterless constructor for {type}");
            var properties = type.GetProperties().Where(IsValidProperty).ToArray();
            var parameterInfos = properties.Select(p => GetParameterInfoForClassProperty(p, source, options)).ToArray();
            var stateVariable = Expression.Variable(typeof(byte), "ee_state");
            var propertyStateVariable = Expression.Variable(typeof(byte), "ee_propetyState");
            var resultVariable = Expression.Variable(type, "ee_result");
            var propertyResultVariable = Expression.Variable(typeof(ParameterBindingResult), "ee_propertyResult");

            var ctor = Expression.New(constructor);
            var blockExpressions = new List<Expression>
            {
                Expression.Assign(stateVariable, Expression.Constant(none)),
                Expression.Assign(resultVariable, ctor)
            };
            for (var i = 0; i < properties.Length; i++)
            {
                blockExpressions.AddRange(BuildPropertyAssignment(properties[i], parameterInfos[i].factory, resultVariable, stateVariable, propertyResultVariable, propertyStateVariable));
            }

            blockExpressions.Add(Expression.New(parameterBindingResultCtor, resultVariable,Expression.Convert(stateVariable, typeof(ParameterBindingIssues))));
            var block = Expression.Block(typeof(ParameterBindingResult),new[] { stateVariable, resultVariable, propertyResultVariable, propertyStateVariable }, blockExpressions);
            var factory = Expression.Lambda<SyncParameterFactory>(block, ctxParameterExpr, optionsParameterExpr, errorCollectionParameterExpr);
            return (factory.Compile(), parameterInfos.SelectMany(s => s.info).ToArray());
        }

        private static Expression[] BuildPropertyAssignment(PropertyInfo property, SyncParameterFactory parameterFactory, ParameterExpression resultVariable, ParameterExpression stateVariable, ParameterExpression propertyResultVariable, ParameterExpression propertyStateVariable)
        {
            var parameterFactoryExpr = Expression.Constant(parameterFactory, typeof(SyncParameterFactory));
            var asignNewPropertyResult = Expression.Assign(
                propertyResultVariable,
                Expression.Call(parameterFactoryExpr, invokeMethod, ctxParameterExpr, optionsParameterExpr, errorCollectionParameterExpr));
            var invokeValue = Expression.Property(propertyResultVariable, parameterBindingResultProperty);
            var invokeToType = Expression.Convert(invokeValue, property.PropertyType);
            var assignPropertyState =Expression.Assign(propertyStateVariable, Expression.Convert(Expression.Property(propertyResultVariable, parameterBindingStateProperty), typeof(byte)));
            var hasError = Expression.And(Expression.Constant(error), propertyStateVariable);
            
            var stateCompare = Expression.Or(stateVariable, hasError);
            var updateState = Expression.Assign(stateVariable, stateCompare);
            var assign = Expression.Assign(Expression.Property(resultVariable, property), invokeToType);
            var ifNoErrorAsign = Expression.IfThen(Expression.Equal(propertyStateVariable, Expression.Constant(none)), assign);
            return new Expression[] 
            {
                asignNewPropertyResult,
                assignPropertyState,
                updateState,
                ifNoErrorAsign
            };
        }

        private static (SyncParameterFactory factory, EndpointParameterDescriptor[] info) GetParameterInfoForClassProperty(PropertyInfo propertyInfo, EndpointParameterSource source, EndpointDeclarationFactoryOptions options)
        {
            return ParameterBinder.GetParameterDeclaration(propertyInfo.Name, propertyInfo.PropertyType, true, null, source, propertyInfo.PropertyType.IsArray, options);
        }

        private static bool IsValidProperty(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite || !propertyInfo.CanRead || propertyInfo.SetMethod is null)
                return false;
            var setter = propertyInfo.SetMethod;
            if (setter.IsPublic)
                return true;
            var setMethodReturnParameterModifiers = setter.ReturnParameter.GetRequiredCustomModifiers();
            return setMethodReturnParameterModifiers.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit));
        }
    }
}
