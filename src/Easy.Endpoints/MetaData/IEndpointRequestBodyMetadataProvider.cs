using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;

namespace Easy.Endpoints
{

    public interface IEndpointRequestBodyMetadataProvider : IFilterMetadata
    {
        Type Type { get; }
        void SetContentTypes(MediaTypeCollection contentTypes);
    }
}
