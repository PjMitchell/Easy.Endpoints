using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;

namespace Easy.Endpoints
{

    internal class JsonEndpointRequestBodyMetaData : IEndpointRequestBodyMetadataProvider
    {
        public JsonEndpointRequestBodyMetaData(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
        public void SetContentTypes(MediaTypeCollection contentTypes)
        {
            contentTypes.Add(new MediaTypeHeaderValue("application/json"));
        }
    }
}
