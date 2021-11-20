using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal static class RouteParameterBinder
    {
        public static bool CanParseRoute(Type type)
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

        public static ParameterFactory GetParameterFactoryForRoute(string parameterName, Type type)
        {
            if(type == typeof(string))
                return GetParameterFactoryForRouteWithStringValue(parameterName);
            if (type == typeof(int))
                return GetParameterFactoryForRoute(parameterName, IntParser.Instance);

            if (type == typeof(long))
                return GetParameterFactoryForRoute(parameterName, LongParser.Instance);

            if (type == typeof(double))
                return GetParameterFactoryForRoute(parameterName, DoubleParser.Instance);

            if (type == typeof(bool))
                return GetParameterFactoryForRoute(parameterName, BoolParser.Instance);

            if (type == typeof(DateTime))
                return GetParameterFactoryForRoute(parameterName, DateTimeParser.Instance);

            if (type == typeof(DateTimeOffset))
                return GetParameterFactoryForRoute(parameterName, DateTimeOffsetParser.Instance);

            if (type == typeof(Guid))
                return GetParameterFactoryForRoute(parameterName, GuidParser.Instance);

            throw new InvalidEndpointSetupException($"Could not parse route value of {type}");
        }

        public static ParameterFactory GetParameterFactoryForRoute<T>(string parameterName, IParsable<T> parsable)
        {
            return (HttpContext ctx) =>
            {
                if (TryGetRouteParameter(ctx, parameterName, out var routeValue) && parsable.TryParse(routeValue, out var result))
                    return ValueTask.FromResult<object?>(result);
                throw new EndpointStatusCodeResponseException(404, "Not Found");
            };

        }

        public static ParameterFactory GetParameterFactoryForRouteWithStringValue(string parameterName)
        {
            return (HttpContext ctx) =>
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
    }
}
