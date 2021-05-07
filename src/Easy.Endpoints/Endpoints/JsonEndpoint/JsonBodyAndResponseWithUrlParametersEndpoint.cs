using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonBodyAndResponseWithUrlParametersEndpoint<THandler, TBody, TUrlParameterModel, TResponse> : IEndpoint
        where THandler : IJsonBodyAndResponseWithUrlParametersEndpointHandler<TBody, TUrlParameterModel, TResponse>
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        private readonly THandler handler;

        public JsonBodyAndResponseWithUrlParametersEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequest(EndpointContext endpointContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(endpointContext).ConfigureAwait(false);
            var parameters = new TUrlParameterModel();
            parameters.BindUrlParameters(endpointContext.Request);
            if(!parameters.IsModelValid())
            {
                await HttpContextJsonHelper.WriteJsonResponse(endpointContext, parameters.Errors, 400).ConfigureAwait(false);
                return;
            }
            var response = await handler.Handle(body, parameters, endpointContext.RequestAborted).ConfigureAwait(false);
            await HttpContextJsonHelper.WriteJsonResponse(endpointContext, response).ConfigureAwait(false);
        }
    }
}
