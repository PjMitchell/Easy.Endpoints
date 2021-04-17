﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public class JsonEndpoint<THandler, TBody, TResponse> : IEndpoint where THandler : IJsonEndpointHandler<TBody, TResponse>
    {
        private readonly THandler handler;

        public JsonEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequest(HttpContext httpContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(httpContext).ConfigureAwait(false);
            if (body is null)
            {
                httpContext.Response.StatusCode = 400;
            }
            else
            {
                var response = await handler.Handle(body).ConfigureAwait(false);
                await HttpContextJsonHelper.WriteJsonResponse(httpContext, response).ConfigureAwait(false);
            }
        }
    }
}
