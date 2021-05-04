using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using static Easy.Endpoints.UrlParameterErrorMessages;
namespace Easy.Endpoints
{
    /// <summary>
    /// Extensions for parsing Guid query parameter 
    /// </summary>
    public static class GuidQueryParameterParserExtensions
    {
        /// <summary>
        /// Tries to parse an Guid query parameter from an IQueryCollection
        /// If no parameters are found it will return success with default value
        /// If multiple values are found it will return as unsuccessful
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="queryCollection">source query collection</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameter</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this IQueryCollection queryCollection, string parameterName, ICollection<UrlParameterModelError> errors, out Guid result)
        {
            if (queryCollection.TryGetValue(parameterName, out var value))
            {
                if (value.Count != 1)
                {
                    result = default;
                    errors.Add(new UrlParameterModelError(parameterName, string.Format(MultipleParametersFoundError, parameterName)));
                    return false;
                }
                if (Guid.TryParse(value[0], out result))
                    return true;
                errors.Add(new UrlParameterModelError(parameterName, string.Format(CouldNotParseError, value[0], typeof(Guid))));
                return false;
            }

            result = default;
            return true;
        }

        /// <summary>
        /// Tries to parse an Guid query parameter from an IQueryCollection
        /// If no parameters are found it will return success with null value
        /// If multiple values are found it will return as unsuccessful
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="queryCollection">source query collection</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameter</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this IQueryCollection queryCollection, string parameterName, ICollection<UrlParameterModelError> errors, out Guid? result)
        {
            if (queryCollection.TryGetValue(parameterName, out var value))
            {
                if (value.Count != 1)
                {
                    result = default;
                    errors.Add(new UrlParameterModelError(parameterName, string.Format(MultipleParametersFoundError, parameterName)));
                    return false;
                }
                if (Guid.TryParse(value[0], out var parsedValue))
                {
                    result = parsedValue;
                    return true;
                }
                errors.Add(new UrlParameterModelError(parameterName, string.Format(CouldNotParseError, value[0], typeof(Guid))));
                result = default;
                return false;
            }

            result = default;
            return true;
        }

        /// <summary>
        /// Tries to parse an Guid array query parameter from an IQueryCollection
        /// If no parameters are found it will return success with empty array
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="queryCollection">source query collection</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameters</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this IQueryCollection queryCollection, string parameterName, ICollection<UrlParameterModelError> errors, out Guid[] result)
        {
            var isValid = true;
            if (queryCollection.TryGetValue(parameterName, out var value))
            {
                result = new Guid[value.Count];
                for (var i = 0; i < value.Count; i++)
                {
                    if (Guid.TryParse(value[i], out var GuidValue))
                    {
                        result[i] = GuidValue;
                    }
                    else
                    {
                        errors.Add(new UrlParameterModelError(parameterName, string.Format(CouldNotParseError, value[i], typeof(Guid))));
                        isValid = false;
                        result[i] = default;
                    }

                }

                return isValid;
            }

            result = Array.Empty<Guid>();
            return true;
        }
    }
}
