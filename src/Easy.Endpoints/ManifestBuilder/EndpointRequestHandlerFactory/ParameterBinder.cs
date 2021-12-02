using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class ParameterBinder
    {
        public static bool CanParseQueryParameter(Type type) => CanParseArrayValue(type) || CanParseNullableValue(type) || CanParseSingleValue(type);
        public static bool CanParseRoute(Type type) => CanParseSingleValue(type);

        public static EndpointParameterInfo GetParameterInfoForRoute(string parameterName, Type type)
        {
            return EndpointParameterInfo.Route(GetRouteParameterFactory(parameterName, type), type, parameterName);
        }

        public static EndpointParameterInfo GetParameterInfoForQuery(string parameterName, Type type, bool hasDefaultValue, object? defaultValue)
        {
            if (type.IsArray)
                return EndpointParameterInfo.Query(GetParameterFactoryForArray(parameterName, type.GetElementType()!), type, parameterName, true);
            return GetParameterInfo(parameterName, type, hasDefaultValue, defaultValue);
        }

        private static EndpointParameterInfo GetParameterInfo(string parameterName, Type type,bool hasDefaultValue, object? defaultValue)
        {
            var parameterType = type;
            var allowNull = hasDefaultValue;
            var isNullable = false;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                isNullable = true;
                allowNull = true;
                if (!hasDefaultValue)
                    defaultValue = null;
                parameterType = type.GetGenericArguments()[0];
            }

            return EndpointParameterInfo.Query(GetParameterFactory(parameterName, parameterType, allowNull, isNullable, defaultValue, ParameterType.Query), type, parameterName, allowNull);

        }

        private static ParameterFactory GetRouteParameterFactory(string parameterName, Type type) => GetParameterFactory(parameterName, type, false, false, null, ParameterType.Route);

        private static ParameterFactory GetParameterFactoryForArray(string parameterName, Type type) => GetParameterFactory(parameterName, type, false, false, null, ParameterType.QueryArray);

        private static ParameterFactory GetParameterFactory(string parameterName, Type type, bool allowNull, bool isNullable, object? defaultValue, ParameterType parameterType)
        {
            ParameterFactory GetParameterFactoryForParser<T>(IParser<T> parsable)
            {
                return GetParameterFactory<T>(parameterName, parsable, allowNull, isNullable, defaultValue, parameterType);
            }


            if (type == typeof(string))
                return GetParameterFactoryForStringValue(parameterName, defaultValue, parameterType);
            if (type == typeof(byte))
                return GetParameterFactoryForParser(NumberParser.Byte);
            if (type == typeof(ushort))
                return GetParameterFactoryForParser(NumberParser.UShort);
            if (type == typeof(short))
                return GetParameterFactoryForParser(NumberParser.Short);
            if (type == typeof(uint))
                return GetParameterFactoryForParser(NumberParser.UInt);
            if (type == typeof(int))
                return GetParameterFactoryForParser(NumberParser.Int);
            if (type == typeof(ulong))
                return GetParameterFactoryForParser(NumberParser.ULong);
            if (type == typeof(long))
                return GetParameterFactoryForParser(NumberParser.Long);
            if (type == typeof(float))
                return GetParameterFactoryForParser(NumberParser.Float);
            if (type == typeof(double))
                return GetParameterFactoryForParser(NumberParser.Double);
            if (type == typeof(decimal))
                return GetParameterFactoryForParser(NumberParser.Decimal);
            if (type == typeof(bool))
                return GetParameterFactoryForParser(BoolParser.Instance);
            if (type == typeof(DateTime))
                return GetParameterFactoryForParser(DateTimeParser.DateTime);
            if (type == typeof(DateTimeOffset))
                return GetParameterFactoryForParser(DateTimeParser.DateTimeOffset);
            if (type == typeof(DateOnly))
                return GetParameterFactoryForParser(DateTimeParser.DateOnly);
            if (type == typeof(TimeOnly))
                return GetParameterFactoryForParser(DateTimeParser.TimeOnly);
            if (type == typeof(Guid))
                return GetParameterFactoryForParser(GuidParser.Instance);

            throw new InvalidEndpointSetupException($"Could not parse route ({parameterName}), cannot parse {type}");
        }
        
        private static ParameterFactory GetParameterFactory<T>(string parameterName, IParser<T> parsable, bool allowNull, bool isNullable, object? defaultValue, ParameterType parameterType)
        {
            return parameterType switch
            {
                ParameterType.Query => GetParameterFactoryForQuery<T>(parameterName, parsable, allowNull, isNullable, defaultValue),
                ParameterType.QueryArray => GetParameterFactoryForQueryArray<T>(parameterName, parsable),
                ParameterType.Route => GetParameterFactoryForRoute<T>(parameterName, parsable),
                _ => throw new ArgumentOutOfRangeException(nameof(parameterType))
            };
        }

        private static ParameterFactory GetParameterFactoryForStringValue(string parameterName, object? defaultValue, ParameterType parameterType)
        {
            return parameterType switch
            {
                ParameterType.Query => GetParameterFactoryForQueryWithStringValue(parameterName, defaultValue),
                ParameterType.QueryArray => GetParameterFactoryForQueryWithStringArrayValue(parameterName),
                ParameterType.Route => GetParameterFactoryForRouteWithStringValue(parameterName),
                _ => throw new ArgumentOutOfRangeException(nameof(parameterType))
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

        private static ParameterFactory GetParameterFactoryForQueryWithStringArrayValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
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

        private static bool CanParseSingleValue(Type type)
        {
            return type == typeof(string)
                || NumberParser.CanParse(type)
                || DateTimeParser.CanParse(type)
                || type == typeof(bool)
                || type == typeof(Guid);
        }

        private static bool CanParseNullableValue(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && CanParseSingleValue(type.GetGenericArguments()[0]);
        }

        private static bool CanParseArrayValue(Type type)
        {

            return type.IsArray && CanParseSingleValue(type.GetElementType());
        }

        private enum ParameterType
        {
            Route = 0,
            Query = 1,
            QueryArray = 2,
        }

    }
}
