using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonBodyWithUrlParametersEndpointResultHandlerEndpoint<THandler, TBody, TUrlParameterModel> : IEndpoint where THandler : IJsonBodyWithUrlParametersEndpointResultHandler<TBody, TUrlParameterModel>
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        private readonly THandler handler;

        public JsonBodyWithUrlParametersEndpointResultHandlerEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequestAsync(EndpointContext endpointContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(endpointContext).ConfigureAwait(false);
            var parameters = new TUrlParameterModel();
            parameters.BindUrlParameters(endpointContext.Request);
            if (!parameters.IsModelValid())
            {
                await HttpContextJsonHelper.WriteJsonResponse(endpointContext, parameters.Errors, 400).ConfigureAwait(false);
                return;
            }
            var endpointResult = await handler.HandleAsync(body, parameters, endpointContext.RequestAborted).ConfigureAwait(false);
            await endpointResult.ExecuteResultAsync(endpointContext).ConfigureAwait(false);
        }
    }
}
