using System;

namespace Easy.Endpoints
{
    internal class JsonEndpointResponseMetaData : EndpointResponseMetaData
    {
        public JsonEndpointResponseMetaData(int statusCode, Type responseType) : base(statusCode, responseType, "application/json")
        {
        }
    }

    internal class PlainTextEndpointResponseMetaData : EndpointResponseMetaData
    {
        public PlainTextEndpointResponseMetaData(int statusCode) : base(statusCode, typeof(string), "text/plain")
        {
        }
    }
}
