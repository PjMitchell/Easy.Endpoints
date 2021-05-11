using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonResponseEndpoint<THandler, TResponse> : IEndpoint where THandler : IJsonResponseEndpointHandler<TResponse>
    {
        private readonly THandler handler;

        public JsonResponseEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequestAsync(EndpointContext endpointContext)
        {
            var response = await handler.HandleAsync(endpointContext.RequestAborted).ConfigureAwait(false);
            await HttpContextJsonHelper.WriteJsonResponse(endpointContext, response).ConfigureAwait(false);
        }
    }
}
