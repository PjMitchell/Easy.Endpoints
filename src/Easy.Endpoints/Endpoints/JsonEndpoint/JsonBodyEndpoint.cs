using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonBodyEndpoint<THandler, TBody> : IEndpoint where THandler : IJsonBodyEndpointHandler<TBody>
    {
        private readonly THandler handler;

        public JsonBodyEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequest(EndpointContext httpContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(httpContext).ConfigureAwait(false);            
            await handler.Handle(body, httpContext.HttpContext.RequestAborted).ConfigureAwait(false);
            httpContext.HttpContext.Response.StatusCode = 201;
            
        }
    }
}
