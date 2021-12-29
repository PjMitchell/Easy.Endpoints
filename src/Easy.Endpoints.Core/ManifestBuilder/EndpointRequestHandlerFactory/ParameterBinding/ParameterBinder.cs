using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Easy.Endpoints
{
    internal static class ParameterBinder
    {
        public static bool CanParseQueryParameter(Type type, EndpointDeclarationFactoryOptions options) => CanParseArrayValue(type, options) || CanParseNullableValue(type, options) || CanParseSingleValue(type, options);
        public static bool CanParseRoute(Type type, EndpointDeclarationFactoryOptions options) => CanParseSingleValue(type, options);

        public static EndpointHandlerParameterDeclaration GetParameterDeclarationForRoute(string parameterName, Type type, EndpointDeclarationFactoryOptions options)
        {
            var (factory, infos) = GetParameterDeclaration(parameterName, type, false, null, EndpointParameterSource.Route, type.IsArray, options);
            return new SyncEndpointParameterDeclaration(factory, infos);
        }

        public static EndpointHandlerParameterDeclaration GetParameterDeclarationForBindingAttribute(IParameterBindingSourceWithNameAttribute parameterBindingSourceWithNameAttribute, string parameterName, Type type, bool hasDefaultValue, object? defaultValue, EndpointDeclarationFactoryOptions options)
        {
            var parameterNameToBind = parameterBindingSourceWithNameAttribute.Name ?? parameterName;
            var (factory, infos) = GetParameterDeclaration(parameterNameToBind, type, hasDefaultValue, defaultValue, parameterBindingSourceWithNameAttribute.Source, type.IsArray, options);
            return new SyncEndpointParameterDeclaration(factory, infos);
        }

        public static EndpointHandlerParameterDeclaration GetParameterDeclarationForQuery(string parameterName, Type type, bool hasDefaultValue, object? defaultValue, EndpointDeclarationFactoryOptions options)
        {
            var (factory, infos) = GetParameterDeclaration(parameterName, type, hasDefaultValue, defaultValue, EndpointParameterSource.Query, type.IsArray, options);
            return new SyncEndpointParameterDeclaration(factory, infos);
        }

        public static (SyncParameterFactory factory, EndpointParameterDescriptor[] info) GetParameterDeclaration(string parameterName, Type type, bool hasDefaultValue, object? defaultValue, EndpointParameterSource paramterSource, bool isArray, EndpointDeclarationFactoryOptions options)
        {
            if (IsComplex(type, options))
                return ComplexParameterBinder.GetParameterDeclarationForClass(type, paramterSource, options);
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

        private static bool IsComplex(Type type, EndpointDeclarationFactoryOptions options)
        {
            if (type.IsArray || type == typeof(string))
                return false;
            return !CanParseQueryParameter(type, options);
        }

        private static SyncParameterFactory GetParameterFactory(string parameterName, Type type, bool allowNull, bool isNullable, object? defaultValue, EndpointParameterSource parameterSource, bool isArray, EndpointDeclarationFactoryOptions options)
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
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                {
                    var state = ParameterBindingIssues.None;
                    if (results.Count != 1)
                    {
                        state = ParameterBindingIssues.Error;
                        bindingErrors.AddMultipleWhenExpectingSingle(parameterName);
                    }

                    if (parsable.TryParse(results[0], opts.FormatProvider, out var result))
                        return new ParameterBindingResult(result, state);
                    bindingErrors.AddCouldNotParseError(parameterName, results[0]);
                    return new ParameterBindingResult(defaultIfNoQueryFound, ParameterBindingIssues.Error);
                }
                if (allowNull)
                    return new ParameterBindingResult(defaultIfNoQueryFound, ParameterBindingIssues.Missing);
                bindingErrors.AddMissingNonNullableValue(parameterName);
                return new ParameterBindingResult(defaultIfNoQueryFound, ParameterBindingIssues.Missing | ParameterBindingIssues.Error);

            };

        }

        private static SyncParameterFactory GetParameterFactoryForHeader<T>(string parameterName, IParser<T> parsable, bool allowNull, bool isNullable, object? defaultValue)
        {
            var defaultIfNoQueryFound = isNullable ? defaultValue : defaultValue ?? default(T);
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (ctx.Request.Headers.TryGetValue(parameterName, out var results))
                {
                    var state = ParameterBindingIssues.None;
                    if(results.Count != 1)
                    {
                        state = ParameterBindingIssues.Error;
                        bindingErrors.AddMultipleWhenExpectingSingle(parameterName);
                    }

                    if (parsable.TryParse(results[0], opts.FormatProvider, out var result))
                        return new ParameterBindingResult(result, state);
                    bindingErrors.AddCouldNotParseError(parameterName, results[0]);
                    return new ParameterBindingResult(defaultIfNoQueryFound, ParameterBindingIssues.Error);
                }
                if (allowNull)
                    return new ParameterBindingResult(defaultIfNoQueryFound, ParameterBindingIssues.Missing);
                bindingErrors.AddMissingNonNullableValue(parameterName);
                return new ParameterBindingResult(defaultIfNoQueryFound, ParameterBindingIssues.Missing | ParameterBindingIssues.Error);
            };

        }

        private static SyncParameterFactory GetParameterFactoryForQueryArray<T>(string parameterName, IParser<T> parsable)
        {
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                {
                    var state = ParameterBindingIssues.None;
                    var parsedResults = new T[results.Count];
                    for (var i = 0; i < results.Count; i++)
                    {
                        if (parsable.TryParse(results[i], opts.FormatProvider, out var result))
                            parsedResults[i] = result;
                        else
                        {
                            bindingErrors.AddCouldNotParseError($"{parameterName}[{i}]", results[i]);
                            state = ParameterBindingIssues.Error;
                        }
                    }

                    return new ParameterBindingResult { Result = parsedResults, State = state };

                }
                return new ParameterBindingResult { Result = Array.Empty<T>() };

            };

        }

        private static SyncParameterFactory GetParameterFactoryForHeaderArray<T>(string parameterName, IParser<T> parsable)
        {
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (ctx.Request.Headers.TryGetValue(parameterName, out var results))
                {
                    var state = ParameterBindingIssues.None;
                    var parsedResults = new T[results.Count];
                    for (var i = 0; i < results.Count; i++)
                    {
                        if (parsable.TryParse(results[i], opts.FormatProvider, out var result))
                            parsedResults[i] = result;
                        else
                        {
                            bindingErrors.AddCouldNotParseError($"{parameterName}[{i}]", results[i]);
                            state = ParameterBindingIssues.Error;
                        }
                    }

                    return new ParameterBindingResult { Result = parsedResults, State = state };

                }
                return new ParameterBindingResult { Result = Array.Empty<T>() };

            };

        }

        public static SyncParameterFactory GetParameterFactoryForQueryWithStringValue(string parameterName, object? defaultValue)
        {
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (!ctx.Request.Query.TryGetValue(parameterName, out var results))
                    return new ParameterBindingResult(defaultValue, ParameterBindingIssues.Missing);
                if (results.Count == 1)
                    return new ParameterBindingResult(results[0]);
                bindingErrors.AddMultipleWhenExpectingSingle(parameterName);
                return new ParameterBindingResult(results[0], ParameterBindingIssues.Error);
            };

        }

        public static SyncParameterFactory GetParameterFactoryForHeaderStringValue(string parameterName, object? defaultValue)
        {
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (!ctx.Request.Headers.TryGetValue(parameterName, out var results))
                    return new ParameterBindingResult(defaultValue, ParameterBindingIssues.Missing);
                if (results.Count == 1)
                    return new ParameterBindingResult(results[0]);
                bindingErrors.AddMultipleWhenExpectingSingle(parameterName);
                return new ParameterBindingResult(results[0], ParameterBindingIssues.Error);
            };

        }

        private static SyncParameterFactory GetParameterFactoryForQueryWithStringArrayValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (ctx.Request.Query.TryGetValue(parameterName, out var results))
                    return new ParameterBindingResult(results.ToArray());
                return new ParameterBindingResult(Array.Empty<string>());
            };

        }

        private static SyncParameterFactory GetParameterFactoryForHeaderWithStringArrayValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (ctx.Request.Headers.TryGetValue(parameterName, out var results))
                    return new ParameterBindingResult(results.ToArray());
                return new  ParameterBindingResult(Array.Empty<string>());
            };

        }

        private static SyncParameterFactory GetParameterFactoryForRoute<T>(string parameterName, IParser<T> parsable)
        {
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (TryGetRouteParameter(ctx, parameterName, out var routeValue) && parsable.TryParse(routeValue, opts.FormatProvider, out var result))
                    return new ParameterBindingResult(result);
                throw new EndpointNotFoundException();
            };

        }

        private static SyncParameterFactory GetParameterFactoryForRouteWithStringValue(string parameterName)
        {
            return (HttpContext ctx, EndpointOptions opts, IBindingErrorCollection bindingErrors) =>
            {
                if (TryGetRouteParameter(ctx, parameterName, out var result))
                    return new ParameterBindingResult(result);
                throw new EndpointNotFoundException();
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

        private static bool CanParseSingleValue(Type type, EndpointDeclarationFactoryOptions options)
        {
            return type == typeof(string)
                || options.Parsers.HasParser(type);
        }

        private static bool CanParseNullableValue(Type type, EndpointDeclarationFactoryOptions options)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && CanParseSingleValue(type.GetGenericArguments()[0], options);
        }

        private static bool CanParseArrayValue(Type type, EndpointDeclarationFactoryOptions options)
        {
            return type.IsArray && CanParseSingleValue(type.GetElementType()!, options);
        }
    }
}
