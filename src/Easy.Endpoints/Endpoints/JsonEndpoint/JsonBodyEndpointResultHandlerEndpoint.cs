using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonBodyEndpointResultHandlerEndpoint<THandler, TBody> : IEndpoint where THandler : IJsonBodyEndpointResultHandler<TBody>
    {
        private readonly THandler handler;

        public JsonBodyEndpointResultHandlerEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequestAsync(EndpointContext endpointContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(endpointContext).ConfigureAwait(false);
            var endpointResult = await handler.HandleAsync(body, endpointContext.RequestAborted).ConfigureAwait(false);
            await endpointResult.ExecuteResultAsync(endpointContext).ConfigureAwait(false);
        }
    }
}
