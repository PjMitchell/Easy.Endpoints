using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonBodyAndResponseEndpoint<THandler, TBody, TResponse> : IEndpoint where THandler : IJsonBodyAndResponseEndpointHandler<TBody, TResponse>
    {
        private readonly THandler handler;

        public JsonBodyAndResponseEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequest(EndpointContext endpointContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(endpointContext).ConfigureAwait(false);
            var response = await handler.Handle(body, endpointContext.RequestAborted).ConfigureAwait(false);
            await HttpContextJsonHelper.WriteJsonResponse(endpointContext, response).ConfigureAwait(false);            
        }
    }
}
