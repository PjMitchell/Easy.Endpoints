using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class ParameterBinder
    {
        public static bool CanParseQueryParameter(Type type, EndpointOptions options) => CanParseArrayValue(type, options) || CanParseNullableValue(type, options) || CanParseSingleValue(type, options);
        public static bool CanParseRoute(Type type, EndpointOptions options) => CanParseSingleValue(type, options);

        public static EndpointParameterInfo GetParameterInfoForRoute(string parameterName, Type type, EndpointOptions options)
        {
            return GetParameterInfo(parameterName, type, false, null, EndpointParameterSource.Route, type.IsArray, options);
        }

        public static EndpointParameterInfo GetParameterInfoForBindingAttribute(IParameterBindingSourceWithNameAttribute parameterBindingSourceWithNameAttribute, string parameterName, Type type, bool hasDefaultValue, object? defaultValue, EndpointOptions options)
        {
            var parameterNameToBind = parameterBindingSourceWithNameAttribute.Name ?? parameterName;
            return GetParameterInfo(parameterNameToBind, type, hasDefaultValue, defaultValue, parameterBindingSourceWithNameAttribute.Source, type.IsArray, options);
        }

        public static EndpointParameterInfo GetParameterInfoForQuery(string parameterName, Type type, bool hasDefaultValue, object? defaultValue, EndpointOptions options)
        {
            return GetParameterInfo(parameterName, type, hasDefaultValue, defaultValue, EndpointParameterSource.Query, type.IsArray, options);
        }

        private static EndpointParameterInfo GetParameterInfo(string parameterName,Type type, bool hasDefaultValue, object? defaultValue, EndpointParameterSource paramterSource, bool isArray, EndpointOptions options)
        {
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
            if(isArray)
            {
                allowNull = false;
                parameterType = type.GetElementType()!;
            }
            var parameterFactory = GetParameterFactory(parameterName, parameterType, allowNull, isNullable, defaultValue, paramterSource, isArray, options);
            return new EndpointParameterInfo(paramterSource, parameterFactory, type, parameterName, allowNull);

        }

        private static ParameterFactory GetParameterFactory(string parameterName, Type type, bool allowNull, bool isNullable, object? defaultValue, EndpointParameterSource parameterSource, bool isArray, EndpointOptions options)
        {
            if (type == typeof(string))
                return GetParameterFactoryForStringValue(parameterName, defaultValue, parameterSource, isArray);
            
            if(!options.Parsers.TryGetParser(type, out var parser))
                throw new InvalidEndpointSetupException($"Could not parse route ({parameterName}), cannot parse {type}");

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            var method = typeof(ParameterBinder).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                            .Single(w => w.Name == nameof(GetParameterFactory) && w.ContainsGenericParameters)
                            .MakeGenericMethod(type);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

            var result = (ParameterFactory)method.Invoke(null,new[] { parameterName, parser, allowNull, isNullable, defaultValue, parameterSource, isArray }  )!;
            return result;
        }


        private static ParameterFactory GetParameterFactory<T>(string parameterName, IParser<T> parsable, bool allowNull, bool isNullable, object? defaultValue, EndpointParameterSource parameterSource, bool isArray)
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

        private static ParameterFactory GetParameterFactoryForStringValue(string parameterName, object? defaultValue, EndpointParameterSource parameterSource, bool isArray)
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

        private static ParameterFactory GetParameterFactoryForQuery<T>(string parameterName, IParser<T> parsable, bool allowNull,bool isNullable, object? defaultValue)
        {
            var defaultIfNoQueryFound = isNullable ? defaultValue : defaultValue ?? default(T);
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                {
                    if(results.Count == 1 && parsable.TryParse(results[0], opts.FormatProvider, out var result))
                        return ValueTask.FromResult<object?>(result);
                    throw new EndpointStatusCodeResponseException();
                }
                if (allowNull)
                    return ValueTask.FromResult<object?>(defaultIfNoQueryFound);
                throw new EndpointStatusCodeResponseException();

            };

        }

        private static ParameterFactory GetParameterFactoryForHeader<T>(string parameterName, IParser<T> parsable, bool allowNull, bool isNullable, object? defaultValue)
        {
            var defaultIfNoQueryFound = isNullable ? defaultValue : defaultValue ?? default(T);
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Headers.TryGetValue(parameterName, out var results))
                {
                    if (results.Count == 1 && parsable.TryParse(results[0], opts.FormatProvider, out var result))
                        return ValueTask.FromResult<object?>(result);
                    throw new EndpointStatusCodeResponseException();
                }
                if (allowNull)
                    return ValueTask.FromResult<object?>(defaultIfNoQueryFound);
                throw new EndpointStatusCodeResponseException();

            };

        }

        private static ParameterFactory GetParameterFactoryForQueryArray<T>(string parameterName, IParser<T> parsable)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                {
                    var parsedResults = new T[results.Count];
                    for(var i = 0; i < results.Count;i++)
                    {
                        if (parsable.TryParse(results[i], opts.FormatProvider, out var result))
                            parsedResults[i] = result;
                        else
                            throw new EndpointStatusCodeResponseException();
                    }
                    
                    return ValueTask.FromResult<object?>(parsedResults);
                    
                }
                return ValueTask.FromResult<object?>(Array.Empty<T>());

            };

        }

        private static ParameterFactory GetParameterFactoryForHeaderArray<T>(string parameterName, IParser<T> parsable)
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

                    return ValueTask.FromResult<object?>(parsedResults);

                }
                return ValueTask.FromResult<object?>(Array.Empty<T>());

            };

        }

        public static ParameterFactory GetParameterFactoryForQueryWithStringValue(string parameterName, object? defaultValue)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (!ctx.Request.Query.TryGetValue(parameterName, out var results))
                    return ValueTask.FromResult<object?>(defaultValue);
                if(results.Count == 1)
                    return ValueTask.FromResult<object?>(results[0]);
                throw new EndpointStatusCodeResponseException();
            };

        }

        public static ParameterFactory GetParameterFactoryForHeaderStringValue(string parameterName, object? defaultValue)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (!ctx.Request.Headers.TryGetValue(parameterName, out var results))
                    return ValueTask.FromResult<object?>(defaultValue);
                if (results.Count == 1)
                    return ValueTask.FromResult<object?>(results[0]);
                throw new EndpointStatusCodeResponseException();
            };

        }

        private static ParameterFactory GetParameterFactoryForQueryWithStringArrayValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                    return ValueTask.FromResult<object?>(results.ToArray());
                return ValueTask.FromResult<object?>(Array.Empty<string>());
            };

        }

        private static ParameterFactory GetParameterFactoryForHeaderWithStringArrayValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Headers.TryGetValue(parameterName, out var results))
                    return ValueTask.FromResult<object?>(results.ToArray());
                return ValueTask.FromResult<object?>(Array.Empty<string>());
            };

        }

        private static ParameterFactory GetParameterFactoryForRoute<T>(string parameterName, IParser<T> parsable)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (TryGetRouteParameter(ctx, parameterName, out var routeValue) && parsable.TryParse(routeValue, opts.FormatProvider, out var result))
                    return ValueTask.FromResult<object?>(result);
                throw new EndpointStatusCodeResponseException(404, "Not Found");
            };

        }

        private static ParameterFactory GetParameterFactoryForRouteWithStringValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (TryGetRouteParameter(ctx, parameterName, out var result))
                    return ValueTask.FromResult<object?>(result);
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
    }
}
