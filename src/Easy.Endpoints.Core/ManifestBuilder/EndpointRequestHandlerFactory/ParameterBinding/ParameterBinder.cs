using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class ParameterBinder
    {
        public static bool CanParseQueryParameter(Type type, EndpointOptions options) => CanParseArrayValue(type, options) || CanParseNullableValue(type, options) || CanParseSingleValue(type, options);
        public static bool CanParseRoute(Type type, EndpointOptions options) => CanParseSingleValue(type, options);

        public static EndpointHandlerParameterDeclaration GetParameterDeclarationForRoute(string parameterName, Type type, EndpointOptions options)
        {
            var (factory, infos) = GetParameterDeclaration(parameterName, type, false, null, EndpointParameterSource.Route, type.IsArray, options);
            return new SyncEndpointParameterDeclaration(factory, infos);
        }

        public static EndpointHandlerParameterDeclaration GetParameterDeclarationForBindingAttribute(IParameterBindingSourceWithNameAttribute parameterBindingSourceWithNameAttribute, string parameterName, Type type, bool hasDefaultValue, object? defaultValue, EndpointOptions options)
        {
            var parameterNameToBind = parameterBindingSourceWithNameAttribute.Name ?? parameterName;
            var (factory, infos) = GetParameterDeclaration(parameterNameToBind, type, hasDefaultValue, defaultValue, parameterBindingSourceWithNameAttribute.Source, type.IsArray, options);
            return new SyncEndpointParameterDeclaration(factory, infos);
        }

        public static EndpointHandlerParameterDeclaration GetParameterDeclarationForQuery(string parameterName, Type type, bool hasDefaultValue, object? defaultValue, EndpointOptions options)
        {
            var (factory, infos) = GetParameterDeclaration(parameterName, type, hasDefaultValue, defaultValue, EndpointParameterSource.Query, type.IsArray, options);
            return new SyncEndpointParameterDeclaration(factory, infos);
        }

        private static (SyncParameterFactory factory, EndpointParameterDescriptor[] info) GetParameterDeclaration(string parameterName, Type type, bool hasDefaultValue, object? defaultValue, EndpointParameterSource paramterSource, bool isArray, EndpointOptions options)
        {
            if (IsComplex(type, options))
                return GetParameterInfoForClass(type, paramterSource, options);
            var parameterType = type;
            var allowNull = hasDefaultValue;
            var isNullable = false;
            if (!isArray && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                isNullable = true;
                allowNull = true;
                if (!hasDefaultValue)
                    defaultValue = null;
                parameterType = type.GetGenericArguments()[0];
            }
            if (isArray)
            {
                allowNull = false;
                parameterType = type.GetElementType()!;
            }
            var parameterFactory = GetParameterFactory(parameterName, parameterType, allowNull, isNullable, defaultValue, paramterSource, isArray, options);
            return (parameterFactory, new[] { new EndpointParameterDescriptor(paramterSource, type, parameterName, allowNull) } );

        }

        private static bool IsComplex(Type type, EndpointOptions options)
        {
            if (type.IsArray || type == typeof(string))
                return false;
            return !CanParseQueryParameter(type, options);
        }

        private static SyncParameterFactory GetParameterFactory(string parameterName, Type type, bool allowNull, bool isNullable, object? defaultValue, EndpointParameterSource parameterSource, bool isArray, EndpointOptions options)
        {
            if (type == typeof(string))
                return GetParameterFactoryForStringValue(parameterName, defaultValue, parameterSource, isArray);

            if (!options.Parsers.TryGetParser(type, out var parser))
                throw new InvalidEndpointSetupException($"Could not parse route ({parameterName}), cannot parse {type}");

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            var method = typeof(ParameterBinder).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                            .Single(w => w.Name == nameof(GetParameterFactory) && w.ContainsGenericParameters)
                            .MakeGenericMethod(type);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

            var result = (SyncParameterFactory)method.Invoke(null, new[] { parameterName, parser, allowNull, isNullable, defaultValue, parameterSource, isArray })!;
            return result;
        }


        private static SyncParameterFactory GetParameterFactory<T>(string parameterName, IParser<T> parsable, bool allowNull, bool isNullable, object? defaultValue, EndpointParameterSource parameterSource, bool isArray)
        {
            return (parameterSource, isArray) switch
            {
                (EndpointParameterSource.Query, false) => GetParameterFactoryForQuery<T>(parameterName, parsable, allowNull, isNullable, defaultValue),
                (EndpointParameterSource.Query, true) => GetParameterFactoryForQueryArray<T>(parameterName, parsable),
                (EndpointParameterSource.Header, false) => GetParameterFactoryForHeader<T>(parameterName, parsable, allowNull, isNullable, defaultValue),
                (EndpointParameterSource.Header, true) => GetParameterFactoryForHeaderArray<T>(parameterName, parsable),
                (EndpointParameterSource.Route, false) => GetParameterFactoryForRoute<T>(parameterName, parsable),
                _ => throw new InvalidEndpointSetupException($"Cannot bind for {parameterSource} and IsArray {isArray}")
            };
        }

        private static SyncParameterFactory GetParameterFactoryForStringValue(string parameterName, object? defaultValue, EndpointParameterSource parameterSource, bool isArray)
        {
            return (parameterSource, isArray) switch
            {
                (EndpointParameterSource.Query, false) => GetParameterFactoryForQueryWithStringValue(parameterName, defaultValue),
                (EndpointParameterSource.Query, true) => GetParameterFactoryForQueryWithStringArrayValue(parameterName),
                (EndpointParameterSource.Header, false) => GetParameterFactoryForHeaderStringValue(parameterName, defaultValue),
                (EndpointParameterSource.Header, true) => GetParameterFactoryForHeaderWithStringArrayValue(parameterName),
                (EndpointParameterSource.Route, false) => GetParameterFactoryForRouteWithStringValue(parameterName),
                _ => throw new InvalidEndpointSetupException($"Cannot bind for {parameterSource} and IsArray {isArray}")
            };
        }

        private static SyncParameterFactory GetParameterFactoryForQuery<T>(string parameterName, IParser<T> parsable, bool allowNull, bool isNullable, object? defaultValue)
        {
            var defaultIfNoQueryFound = isNullable ? defaultValue : defaultValue ?? default(T);
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                {
                    if (results.Count == 1 && parsable.TryParse(results[0], opts.FormatProvider, out var result))
                        return result;
                    throw new EndpointStatusCodeResponseException();
                }
                if (allowNull)
                    return defaultIfNoQueryFound;
                throw new EndpointStatusCodeResponseException();

            };

        }

        private static SyncParameterFactory GetParameterFactoryForHeader<T>(string parameterName, IParser<T> parsable, bool allowNull, bool isNullable, object? defaultValue)
        {
            var defaultIfNoQueryFound = isNullable ? defaultValue : defaultValue ?? default(T);
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Headers.TryGetValue(parameterName, out var results))
                {
                    if (results.Count == 1 && parsable.TryParse(results[0], opts.FormatProvider, out var result))
                        return result;
                    throw new EndpointStatusCodeResponseException();
                }
                if (allowNull)
                    return defaultIfNoQueryFound;
                throw new EndpointStatusCodeResponseException();

            };

        }

        private static SyncParameterFactory GetParameterFactoryForQueryArray<T>(string parameterName, IParser<T> parsable)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                {
                    var parsedResults = new T[results.Count];
                    for (var i = 0; i < results.Count; i++)
                    {
                        if (parsable.TryParse(results[i], opts.FormatProvider, out var result))
                            parsedResults[i] = result;
                        else
                            throw new EndpointStatusCodeResponseException();
                    }

                    return parsedResults;

                }
                return Array.Empty<T>();

            };

        }

        private static SyncParameterFactory GetParameterFactoryForHeaderArray<T>(string parameterName, IParser<T> parsable)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Headers.TryGetValue(parameterName, out var results))
                {
                    var parsedResults = new T[results.Count];
                    for (var i = 0; i < results.Count; i++)
                    {
                        if (parsable.TryParse(results[i], opts.FormatProvider, out var result))
                            parsedResults[i] = result;
                        else
                            throw new EndpointStatusCodeResponseException();
                    }

                    return parsedResults;

                }
                return Array.Empty<T>();

            };

        }

        public static SyncParameterFactory GetParameterFactoryForQueryWithStringValue(string parameterName, object? defaultValue)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (!ctx.Request.Query.TryGetValue(parameterName, out var results))
                    return defaultValue;
                if (results.Count == 1)
                    return results[0];
                throw new EndpointStatusCodeResponseException();
            };

        }

        public static SyncParameterFactory GetParameterFactoryForHeaderStringValue(string parameterName, object? defaultValue)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (!ctx.Request.Headers.TryGetValue(parameterName, out var results))
                    return defaultValue;
                if (results.Count == 1)
                    return results[0];
                throw new EndpointStatusCodeResponseException();
            };

        }

        private static SyncParameterFactory GetParameterFactoryForQueryWithStringArrayValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                    return results.ToArray();
                return Array.Empty<string>();
            };

        }

        private static SyncParameterFactory GetParameterFactoryForHeaderWithStringArrayValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Headers.TryGetValue(parameterName, out var results))
                    return results.ToArray();
                return Array.Empty<string>();
            };

        }

        private static SyncParameterFactory GetParameterFactoryForRoute<T>(string parameterName, IParser<T> parsable)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (TryGetRouteParameter(ctx, parameterName, out var routeValue) && parsable.TryParse(routeValue, opts.FormatProvider, out var result))
                    return result;
                throw new EndpointStatusCodeResponseException(404, "Not Found");
            };

        }

        private static SyncParameterFactory GetParameterFactoryForRouteWithStringValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (TryGetRouteParameter(ctx, parameterName, out var result))
                    return result;
                throw new EndpointStatusCodeResponseException(404, "Not Found");
            };

        }

        public static bool TryGetRouteParameter(this HttpContext ctx, string parameterName, out string result)
        {
            if (ctx.Request.RouteValues.TryGetValue(parameterName, out var value) && value is string stringValue)
            {
                result = stringValue;
                return true;
            }
            result = string.Empty;
            return false;
        }

        private static bool CanParseSingleValue(Type type, EndpointOptions options)
        {
            return type == typeof(string)
                || options.Parsers.HasParser(type);
        }

        private static bool CanParseNullableValue(Type type, EndpointOptions options)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && CanParseSingleValue(type.GetGenericArguments()[0], options);
        }

        private static bool CanParseArrayValue(Type type, EndpointOptions options)
        {
            return type.IsArray && CanParseSingleValue(type.GetElementType()!, options);
        }

        private static (SyncParameterFactory factory, EndpointParameterDescriptor[] info) GetParameterInfoForClass(Type type, EndpointParameterSource source, EndpointOptions options)
        {
            var constructor = type.GetConstructor(Array.Empty<Type>());
            if (constructor is null)
                throw new InvalidEndpointSetupException($"No parameterless constructor for {type}");
            var properties = type.GetProperties().Where(IsValidProperty).ToArray();
            var parameterInfos = properties.Select(p => GetParameterInfoForClassProperty(p, source, options)).ToArray();

            const string ctxParameter = "ee_ctx";
            const string optionParameter = "ee_options";

            var ctor = Expression.New(constructor);
            var bindings = new MemberBinding[properties.Length];
            var ctxParameterExpr = Expression.Parameter(typeof(HttpContext), ctxParameter);
            var optionsParameterExpr = Expression.Parameter(typeof(EndpointOptions), optionParameter);
            var invokeMethod = typeof(SyncParameterFactory).GetMethod("Invoke")!;
            for (var i = 0; i < properties.Length;i++)
            {
                var property = properties[i];
                var (parameterFactory, infos) = parameterInfos[i];
                var parameterFactoryExpr = Expression.Constant(parameterFactory, typeof(SyncParameterFactory));
                var invoke = Expression.Call(parameterFactoryExpr, invokeMethod, ctxParameterExpr, optionsParameterExpr); 
                var invokeToType = Expression.Convert(invoke, property.PropertyType);

                bindings[i] = Expression.Bind(property, invokeToType);
            }
            var memberInit = Expression.MemberInit(ctor, bindings);
            var factory = Expression.Lambda<SyncParameterFactory>(memberInit, ctxParameterExpr, optionsParameterExpr);
            return (factory.Compile(), parameterInfos.SelectMany(s => s.info).ToArray());
        }

        private static (SyncParameterFactory factory, EndpointParameterDescriptor[] info) GetParameterInfoForClassProperty(PropertyInfo propertyInfo, EndpointParameterSource source, EndpointOptions options)
        {
            return GetParameterDeclaration(propertyInfo.Name, propertyInfo.PropertyType, false, null, source, propertyInfo.PropertyType.IsArray, options);
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
