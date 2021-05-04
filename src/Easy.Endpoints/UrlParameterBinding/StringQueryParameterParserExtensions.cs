using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using static Easy.Endpoints.UrlParameterErrorMessages;
namespace Easy.Endpoints
{
    /// <summary>
    /// Helpers for Parsing Query Parameters
    /// </summary>
    public static class StringQueryParameterParserExtensions
    {
        /// <summary>
        /// Tries to parse an string query parameter from an IQueryCollection
        /// If no parameters are found it will return success with default value
        /// If multiple values are found it will return as unsuccessful
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="queryCollection">source query collection</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameter</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this IQueryCollection queryCollection,  string parameterName, ICollection<UrlParameterModelError> errors, out string result)
        {
            if (queryCollection.TryGetValue(parameterName, out var value))
            {
                if(value.Count == 1)
                {
                    result = value[0];
                    return true;
                }
                errors.Add(new UrlParameterModelError(parameterName, string.Format(MultipleParametersFoundError, parameterName)));
                result = string.Empty;
                return false;
            }            
            result = string.Empty;
            return true;
        }

        /// <summary>
        /// Tries to parse an string array query parameter from an IQueryCollection
        /// If no parameters are found it will return success with empty array
        /// Errors will be added to the errors
        /// </summary>
        /// <param name="queryCollection">source query collection</param>
        /// <param name="parameterName">name of parameter to be found</param>
        /// <param name="errors">error collection to record issue</param>
        /// <param name="result">resulting parameters</param>
        /// <returns>returns true if parameter correctly retrieved, false otherwise</returns>
        public static bool TryGetQueryParameter(this IQueryCollection queryCollection, string parameterName, ICollection<UrlParameterModelError> errors, out string[] result)
        {
            if (queryCollection.TryGetValue(parameterName, out var value))
            {
                result = value.ToArray();
                return true;
            }

            result = Array.Empty<string>();
            return true;
        }


    }
}
