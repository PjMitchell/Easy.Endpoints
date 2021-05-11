using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class UrlParametersEndpointResultHandlerEndpoint<THandler, TUrlParameterModel> : IEndpoint where THandler : IUrlParametersEndpointResultHandler<TUrlParameterModel>
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        private readonly THandler handler;

        public UrlParametersEndpointResultHandlerEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequestAsync(EndpointContext endpointContext)
        {
            var parameters = new TUrlParameterModel();
            parameters.BindUrlParameters(endpointContext.Request);
            if (!parameters.IsModelValid())
            {
                await HttpContextJsonHelper.WriteJsonResponse(endpointContext, parameters.Errors, 400).ConfigureAwait(false);
                return;
            }
            var endpointResult = await handler.HandleAsync(parameters, endpointContext.RequestAborted).ConfigureAwait(false);
            await endpointResult.ExecuteResultAsync(endpointContext).ConfigureAwait(false);
        }
    }
}
