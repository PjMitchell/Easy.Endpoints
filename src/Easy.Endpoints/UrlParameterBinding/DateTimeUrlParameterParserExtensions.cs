﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using static Easy.Endpoints.UrlParameterErrorMessages;
namespace Easy.Endpoints
{
    /// <summary>
    /// Extensions for parsing DateTime query parameter 
    /// </summary>
    public static class DateTimeUrlParameterParserExtensions
    {
        /// <summary>
        /// Tries to parse an DateTime query parameter from an IQueryCollection
        /// If no parameters are found it will return success with default value
        /// If multiple values are found it will return as unsuccessful
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="httpRequest">http request</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameter</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this HttpRequest httpRequest, string parameterName, ICollection<UrlParameterModelError> errors, out DateTime result)
        {
            if (httpRequest.Query.TryGetValue(parameterName, out var value))
            {
                if (value.Count != 1)
                {
                    result = default;
                    errors.Add(new UrlParameterModelError(parameterName, string.Format(MultipleParametersFoundError, parameterName)));
                    return false;
                }
                if (DateTime.TryParse(value[0], out result))
                    return true;
                errors.Add(new UrlParameterModelError(parameterName, string.Format(CouldNotParseError, value[0], typeof(DateTime))));
                return false;
            }

            result = default;
            return true;
        }

        /// <summary>
        /// Tries to parse an DateTime query parameter from an IQueryCollection
        /// If no parameters are found it will return success with null value
        /// If multiple values are found it will return as unsuccessful
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="httpRequest">http request</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameter</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this HttpRequest httpRequest, string parameterName, ICollection<UrlParameterModelError> errors, out DateTime? result)
        {
            if (httpRequest.Query.TryGetValue(parameterName, out var value))
            {
                if (value.Count != 1)
                {
                    result = default;
                    errors.Add(new UrlParameterModelError(parameterName, string.Format(MultipleParametersFoundError, parameterName)));
                    return false;
                }
                if (DateTime.TryParse(value[0], out var parsedValue))
                {
                    result = parsedValue;
                    return true;
                }
                errors.Add(new UrlParameterModelError(parameterName, string.Format(CouldNotParseError, value[0], typeof(DateTime))));
                result = default;
                return false;
            }

            result = default;
            return true;
        }

        /// <summary>
        /// Tries to parse an DateTime route parameter from an RouteValueDictionary
        /// </summary>
        /// <param name="httpRequest">http request</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="result">resulting parameter</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetRouteParameter(this HttpRequest httpRequest, string parameterName, out DateTime result)
        {
            if (httpRequest.RouteValues.TryGetValue(parameterName, out var value) && value is string stringValue && DateTime.TryParse(stringValue, out result))
            {
                return true;
            }
            result = default;
            return false;
        }

    }
}