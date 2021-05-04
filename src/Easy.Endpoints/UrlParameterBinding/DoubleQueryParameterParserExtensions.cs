﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using static Easy.Endpoints.UrlParameterErrorMessages;
namespace Easy.Endpoints
{
    /// <summary>
    /// Extensions for parsing double query parameter 
    /// </summary>
    public static class DoubleQueryParameterParserExtensions
    {
        /// <summary>
        /// Tries to parse an double query parameter from an IQueryCollection
        /// If no parameters are found it will return success with default value
        /// If multiple values are found it will return as unsuccessful
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="queryCollection">source query collection</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameter</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this IQueryCollection queryCollection, string parameterName, ICollection<UrlParameterModelError> errors, out double result)
        {
            if (queryCollection.TryGetValue(parameterName, out var value))
            {
                if (value.Count != 1)
                {
                    result = default;
                    errors.Add(new UrlParameterModelError(parameterName, string.Format(MultipleParametersFoundError, parameterName)));
                    return false;
                }
                if (double.TryParse(value[0], out result))
                    return true;
                errors.Add(new UrlParameterModelError(parameterName, string.Format(CouldNotParseError, value[0], typeof(double))));
                return false;
            }

            result = default;
            return true;
        }

        /// <summary>
        /// Tries to parse an double query parameter from an IQueryCollection
        /// If no parameters are found it will return success with null value
        /// If multiple values are found it will return as unsuccessful
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="queryCollection">source query collection</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameter</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this IQueryCollection queryCollection, string parameterName, ICollection<UrlParameterModelError> errors, out double? result)
        {
            if (queryCollection.TryGetValue(parameterName, out var value))
            {
                if (value.Count != 1)
                {
                    result = default;
                    errors.Add(new UrlParameterModelError(parameterName, string.Format(MultipleParametersFoundError, parameterName)));
                    return false;
                }
                if (double.TryParse(value[0], out var parsedValue))
                {
                    result = parsedValue;
                    return true;
                }
                errors.Add(new UrlParameterModelError(parameterName, string.Format(CouldNotParseError, value[0], typeof(double))));
                result = default;
                return false;
            }

            result = default;
            return true;
        }

        /// <summary>
        /// Tries to parse an int array query parameter from an IQueryCollection
        /// If no parameters are found it will return success with empty array
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="queryCollection">source query collection</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameters</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this IQueryCollection queryCollection, string parameterName, ICollection<UrlParameterModelError> errors, out double[] result)
        {
            var isValid = true;
            if (queryCollection.TryGetValue(parameterName, out var value))
            {
                result = new double[value.Count];
                for (var i = 0; i < value.Count; i++)
                {
                    if (int.TryParse(value[i], out var intValue))
                    {
                        result[i] = intValue;
                    }
                    else
                    {
                        errors.Add(new UrlParameterModelError(parameterName, string.Format(CouldNotParseError, value[i], typeof(double))));
                        isValid = false;
                        result[i] = default;
                    }

                }

                return isValid;
            }

            result = Array.Empty<double>();
            return true;
        }
    }
}
