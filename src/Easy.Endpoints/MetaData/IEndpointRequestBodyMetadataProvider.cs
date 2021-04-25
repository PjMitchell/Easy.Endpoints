using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// MetaData used to declare Endpoint request body type
    /// </summary>
    public interface IEndpointRequestBodyMetadataProvider : IFilterMetadata
    {
        /// <summary>
        /// Model for Request Body
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Applies MediaTypes to MediaTypeCollection
        /// </summary>
        /// <param name="contentTypes">MediaTypeCollection to be modified</param>
        void SetContentTypes(MediaTypeCollection contentTypes);
    }
}
