using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Easy.Endpoints
{
    /// <summary>
    /// Helper for Generating UrlParameterModel Bindings
    /// </summary>
    public static class UrlParameterBindingHelper
    {
        /// <summary>
        /// Uses reflection to generate url binding
        /// </summary>
        /// <typeparam name="T">Type of model to be bound</typeparam>
        /// <returns>Binding function</returns>
        public static Action<T, HttpRequest> BuildBinder<T>() where T : notnull, UrlParameterModel
        {
            var bindings = new List<Action<T, HttpRequest>>();
            foreach(var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(i => i.CanWrite))
            {
                if(CanParseSingleValue(property.PropertyType) || CanParseNullableValue(property.PropertyType) || CanParseArrayValue(property.PropertyType))
                    bindings.Add(GetBindingForProperty<T>(property));
            }
            var bindingsAsArray = bindings.ToArray();
            return (T model, HttpRequest r) => {
                foreach (var action in bindingsAsArray)
                    action(model, r);
            };
        }

        private static Action<T, HttpRequest> GetBindingForProperty<T>(PropertyInfo info) where T : notnull, UrlParameterModel
        {
            var parameterName = GetParameterName(info);
            return GetQueryParser<T>(info.PropertyType, parameterName, info.SetValue);
        }

        private static Action<T, HttpRequest> GetQueryParser<T>(Type type, string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            if (type.IsArray)
                return GetArrayParameterQueryParser<T>(type, parameter, setMethod);
            return GetSingleParameterQueryParser<T>(type, parameter, setMethod);
        }

        private static Action<T, HttpRequest> GetArrayParameterQueryParser<T>(Type type, string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            if (type == typeof(string[]))
                return ForQueryStringArray<T>(parameter, setMethod);
            if (type == typeof(int[]))
                return ForQueryIntArray<T>(parameter, setMethod);
            if (type == typeof(long[]))
                return ForQueryLongArray<T>(parameter, setMethod);
            if (type == typeof(double[]))
                return ForQueryDoubleArray<T>(parameter, setMethod);
            if (type == typeof(Guid[]))
                return ForQueryGuidArray<T>(parameter, setMethod);
            throw new InvalidEndpointSetupException($"Cannot map {type}");
        }

        private static Action<T, HttpRequest> GetSingleParameterQueryParser<T>(Type type, string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            if (type == typeof(string))
                return ForQueryString<T>(parameter, setMethod);
            if (type == typeof(int))
                return ForQueryInt<T>(parameter, setMethod);
            if (type == typeof(int?))
                return ForQueryNullableInt<T>(parameter, setMethod);
            if (type == typeof(long))
                return ForQueryLong<T>(parameter, setMethod);
            if (type == typeof(long?))
                return ForQueryNullableLong<T>(parameter, setMethod);
            if (type == typeof(bool))
                return ForQueryBool<T>(parameter, setMethod);
            if (type == typeof(bool?))
                return ForQueryNullableBool<T>(parameter, setMethod);
            if (type == typeof(double))
                return ForQueryDouble<T>(parameter, setMethod);
            if (type == typeof(double?))
                return ForQueryNullableDouble<T>(parameter, setMethod);
            if (type == typeof(DateTime))
                return ForQueryDateTime<T>(parameter, setMethod);
            if (type == typeof(DateTime?))
                return ForQueryNullableDateTime<T>(parameter, setMethod);
            if (type == typeof(DateTimeOffset))
                return ForQueryDateTimeOffset<T>(parameter, setMethod);
            if (type == typeof(DateTimeOffset?))
                return ForQueryNullableDateTimeOffset<T>(parameter, setMethod);
            if (type == typeof(Guid))
                return ForQueryGuid<T>(parameter, setMethod);
            if (type == typeof(Guid?))
                return ForQueryNullableGuid<T>(parameter, setMethod);
            throw new InvalidEndpointSetupException($"Cannot map {type}");
        }

        #region string
        private static Action<T, HttpRequest> ForQueryString<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out string result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryStringArray<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out string[] result))
                {
                    setMethod(model, result);
                }
            };
        }
        #endregion

        #region int
        private static Action<T, HttpRequest> ForQueryInt<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out int result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryNullableInt<T>(string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out int? result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryIntArray<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out int[] result))
                {
                    setMethod(model, result);
                }
            };
        }
        #endregion

        #region long
        private static Action<T, HttpRequest> ForQueryLong<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out long result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryNullableLong<T>(string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out long? result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryLongArray<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out long[] result))
                {
                    setMethod(model, result);
                }
            };
        }
        #endregion

        #region bool
        private static Action<T, HttpRequest> ForQueryBool<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out bool result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryNullableBool<T>(string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out bool? result))
                {
                    setMethod(model, result);
                }
            };
        }
        #endregion

        #region double
        private static Action<T, HttpRequest> ForQueryDouble<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out double result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryNullableDouble<T>(string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out double? result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryDoubleArray<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out double[] result))
                {
                    setMethod(model, result);
                }
            };
        }
        #endregion

        #region guid
        private static Action<T, HttpRequest> ForQueryGuid<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out Guid result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryNullableGuid<T>(string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out Guid? result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryGuidArray<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out Guid[] result))
                {
                    setMethod(model, result);
                }
            };
        }
        #endregion

        #region dateTime
        private static Action<T, HttpRequest> ForQueryDateTime<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out DateTime result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryNullableDateTime<T>(string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out DateTime? result))
                {
                    setMethod(model, result);
                }
            };
        }
        #endregion

        #region dateTimeOffSet
        private static Action<T, HttpRequest> ForQueryDateTimeOffset<T>(string parameter, Action<object, object> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out DateTimeOffset result))
                {
                    setMethod(model, result);
                }
            };
        }

        private static Action<T, HttpRequest> ForQueryNullableDateTimeOffset<T>(string parameter, Action<object, object?> setMethod) where T : notnull, UrlParameterModel
        {
            return (model, request) =>
            {
                if (request.Query.TryGetQueryParameter(parameter, model.Errors, out DateTimeOffset? result))
                {
                    setMethod(model, result);
                }
            };
        }
        #endregion

        private static string GetParameterName(PropertyInfo info)
        {
            var queryAttribute = info.GetCustomAttribute<QueryParameterAttribute>();
            if (queryAttribute is not null && !string.IsNullOrWhiteSpace(queryAttribute.Name))
                return queryAttribute.Name;
            var characters = info.Name.ToCharArray();
            characters[0] = char.ToLower(characters[0]);
            return new string(characters);
        }

        private static bool CanParseSingleValue(Type type)
        {
            return type == typeof(string) 
                || type == typeof(int) 
                || type == typeof(long)
                || type == typeof(bool)
                || type == typeof(double)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(Guid)

                ;
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


    }
}
