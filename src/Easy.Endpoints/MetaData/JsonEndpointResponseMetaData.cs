using System;

namespace Easy.Endpoints
{    
    internal class JsonEndpointResponseMetaData : EndpointResponseMetaData
    {
        public JsonEndpointResponseMetaData(int statusCode, Type responseType) : base(statusCode, responseType, "application/json")
        {
        }
    }
}
