using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class EndpointResultHandlerEndpoint<THandler> : IEndpoint where THandler : IEndpointResultHandler
    {
        private readonly THandler handler;

        public EndpointResultHandlerEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequestAsync(EndpointContext endpointContext)
        {
            var endpointResult = await handler.HandleAsync(endpointContext.RequestAborted).ConfigureAwait(false);
            await endpointResult.ExecuteResultAsync(endpointContext).ConfigureAwait(false);
        }
    }
}
