using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;

namespace Easy.Endpoints
{

    public class EndpointResponseMetaData : IApiResponseMetadataProvider
    {
        private readonly string[] responseTypes;

        public EndpointResponseMetaData(int statusCode, Type responseType, params string[] responseTypes)
        {
            StatusCode = statusCode;
            Type = responseType;
            this.responseTypes = responseTypes;
        }

        public Type Type { get; }
        public int StatusCode { get; }

        public void SetContentTypes(MediaTypeCollection contentTypes)
        {
            foreach (var contentType in responseTypes)
                contentTypes.Add(new MediaTypeHeaderValue(contentType));
        }
    }
}
