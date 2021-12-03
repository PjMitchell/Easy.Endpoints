using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Returns Response Type and code meta data for endpoint
    /// </summary>
    public class EndpointResponseMetaData : IApiResponseMetadataProvider
    {
        private readonly string[] responseTypes;

        /// <summary>
        /// Constructs new instance of EndpointResponseMetaData
        /// </summary>
        /// <param name="statusCode">Status Code of response</param>
        /// <param name="responseType">Model type of response</param>
        /// <param name="responseTypes">Media Type of response</param>
        public EndpointResponseMetaData(int statusCode, Type responseType, params string[] responseTypes)
        {
            StatusCode = statusCode;
            Type = responseType;
            this.responseTypes = responseTypes;
        }

        /// <summary>
        /// Gets Model type of response
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets Status Code of response
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Applies MediaTypes to MediaTypeCollection
        /// </summary>
        /// <param name="contentTypes">MediaTypeCollection to be modified</param>
        public void SetContentTypes(MediaTypeCollection contentTypes)
        {
            foreach (var contentType in responseTypes)
                contentTypes.Add(new MediaTypeHeaderValue(contentType));
        }
    }
}
