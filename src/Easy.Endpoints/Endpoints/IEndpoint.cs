﻿using System.Threading.Tasks;

namespace Easy.Endpoints
{
    /// <summary>
    /// Endpoint for use with Easy.Endpoint
    /// </summary>
    public interface IEndpoint
    {
        /// <summary>
        /// Handles the Http Request
        /// </summary>
        /// <param name="endpointContext">Context of the request</param>
        /// <returns>A task for the operation</returns>
        Task HandleRequest(EndpointContext endpointContext);
    }
}
