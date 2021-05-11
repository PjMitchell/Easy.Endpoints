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

        public async Task HandleRequestAsync(EndpointContext endpointContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(endpointContext).ConfigureAwait(false);
            await handler.HandleAsync(body, endpointContext.RequestAborted).ConfigureAwait(false);
            endpointContext.Response.StatusCode = 201;
        }
    }
}
