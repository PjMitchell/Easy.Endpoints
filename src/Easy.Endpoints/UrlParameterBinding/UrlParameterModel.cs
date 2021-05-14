using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <summary>
    /// Defines a model that will be populated from the url parameters
    /// </summary>
    public abstract class UrlParameterModel
    {
        /// <summary>
        /// Binds the url parameters from the request to the model
        /// </summary>
        /// <param name="request"></param>
        public virtual void BindUrlParameters(HttpRequest request) { }

        /// <summary>
        /// Error on model binding
        /// </summary>
        public ICollection<UrlParameterModelError> Errors { get; } = new List<UrlParameterModelError>();

        /// <summary>
        /// Checks if model is valid
        /// </summary>
        /// <returns>returns true if model is valid and false if not</returns>
        public bool IsModelValid() => Errors.Count == 0;
    }

    /// <summary>
    /// Defines error in UrlParameterModel
    /// </summary>
    public record UrlParameterModelError
    {
        /// <summary>
        /// Constructs new UrlParameterModelError
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="error">Model Error Message</param>
        public UrlParameterModelError(string parameterName, string error)
        {
            ParameterName = parameterName;
            Error = error;
        }

        /// <summary>
        /// Url Parameter that caused the issue
        /// </summary>
        public string ParameterName { get; }
        /// <summary>
        /// Model Error message
        /// </summary>
        public string Error { get;  }

    }
}
