using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class QueryParameterBinder
    {
        public static bool CanParseQueryParameter(Type type) => CanParseArrayValue(type) || CanParseNullableValue(type) || CanParseSingleValue(type);

        private static bool CanParseSingleValue(Type type)
        {
            return type == typeof(string)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(bool)
                || type == typeof(double)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(Guid);
        }

        private static bool CanParseNullableValue(Type type)
        {
            return type == typeof(int?)
                || type == typeof(long?)
                || type == typeof(bool?)
                || type == typeof(double?)
                || type == typeof(DateTime?)
                || type == typeof(DateTimeOffset?)
                || type == typeof(Guid?);
        }

        private static bool CanParseArrayValue(Type type)
        {
            return type == typeof(string[])
                || type == typeof(int[])
                || type == typeof(long[])
                || type == typeof(double[])
                || type == typeof(Guid[]);
        }

        public static ParameterFactory GetParameterFactoryForQuery(string parameterName, Type type, bool hasDefaultValue, object? defaultValue)
        {
            if (type.IsArray)
                return GetArrayParameterFactory(parameterName, type);
            return GetParameterFactory(parameterName, type, hasDefaultValue, defaultValue);
        }

        private static ParameterFactory GetParameterFactory(string parameterName, Type type,bool hasDefaultValue, object? defaultValue)
        {
            var allowNull = hasDefaultValue;
            var isNullable = false;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                isNullable = true;
                allowNull = true;
                if (!hasDefaultValue)
                    defaultValue = null;
                type = type.GetGenericArguments()[0];
            }
            if (type == typeof(string))
                return GetParameterFactoryForQueryWithStringValue(parameterName, defaultValue);
            if (type == typeof(int))
                return GetParameterFactoryForQuery(parameterName, IntParser.Instance, allowNull,isNullable, defaultValue);

            if (type == typeof(long))
                return GetParameterFactoryForQuery(parameterName, LongParser.Instance, allowNull, isNullable, defaultValue);

            if (type == typeof(double))
                return GetParameterFactoryForQuery(parameterName, DoubleParser.Instance, allowNull, isNullable, defaultValue);

            if (type == typeof(bool))
                return GetParameterFactoryForQuery(parameterName, BoolParser.Instance, allowNull, isNullable, defaultValue);

            if (type == typeof(DateTime))
                return GetParameterFactoryForQuery(parameterName, DateTimeParser.Instance, allowNull, isNullable, defaultValue);

            if (type == typeof(DateTimeOffset))
                return GetParameterFactoryForQuery(parameterName, DateTimeOffsetParser.Instance, allowNull, isNullable, defaultValue);

            if (type == typeof(Guid))
                return GetParameterFactoryForQuery(parameterName, GuidParser.Instance, allowNull, isNullable, defaultValue);

            throw new InvalidEndpointSetupException($"Could not parse route ({parameterName}), cannot parse {type}");
        }

        private static ParameterFactory GetArrayParameterFactory(string parameterName, Type type)
        {
            
            if (type == typeof(string[]))
                return GetParameterFactoryForQueryWithStringArrayValue(parameterName);
            if (type == typeof(int[]))
                return GetParameterFactoryForQueryArray(parameterName, IntParser.Instance);

            if (type == typeof(long[]))
                return GetParameterFactoryForQueryArray(parameterName, LongParser.Instance);

            if (type == typeof(double[]))
                return GetParameterFactoryForQueryArray(parameterName, DoubleParser.Instance);

            if (type == typeof(bool[]))
                return GetParameterFactoryForQueryArray(parameterName, BoolParser.Instance);

            if (type == typeof(DateTime[]))
                return GetParameterFactoryForQueryArray(parameterName, DateTimeParser.Instance);

            if (type == typeof(DateTimeOffset[]))
                return GetParameterFactoryForQueryArray(parameterName, DateTimeOffsetParser.Instance);

            if (type == typeof(Guid[]))
                return GetParameterFactoryForQueryArray(parameterName, GuidParser.Instance);

            throw new InvalidEndpointSetupException($"Could not parse route value of {type}");
        }


        private static ParameterFactory GetParameterFactoryForQuery<T>(string parameterName, IParsable<T> parsable, bool allowNull,bool isNullable, object? defaultValue)
        {
            var defaultIfNoQueryFound = isNullable ? defaultValue : defaultValue ?? default(T);
            return (HttpContext ctx) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                {
                    if(results.Count == 1 && parsable.TryParse(results[0], out var result))
                        return ValueTask.FromResult<object?>(result);
                    throw new EndpointStatusCodeResponseException();
                }
                if (allowNull)
                    return ValueTask.FromResult<object?>(defaultIfNoQueryFound);
                throw new EndpointStatusCodeResponseException();

            };

        }

        private static ParameterFactory GetParameterFactoryForQueryArray<T>(string parameterName, IParsable<T> parsable)
        {
            return (HttpContext ctx) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                {
                    var parsedResults = new T[results.Count];
                    for(var i = 0; i < results.Count;i++)
                    {
                        if (parsable.TryParse(results[i], out var result))
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
            return (HttpContext ctx) =>
            {
                if (!ctx.Request.Query.TryGetValue(parameterName, out var results))
                    return ValueTask.FromResult<object?>(defaultValue);
                if(results.Count == 1)
                    return ValueTask.FromResult<object?>(results[0]);
                throw new EndpointStatusCodeResponseException();
            };

        }

        public static ParameterFactory GetParameterFactoryForQueryWithStringArrayValue(string parameterName)
        {
            return (HttpContext ctx) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                    return ValueTask.FromResult<object?>(results.ToArray());
                return ValueTask.FromResult<object?>(Array.Empty<string>());
            };

        }

    }
}
