using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonResponseWithUrlParametersEndpoint<THandler, TResponse, TUrlParameterModel> : IEndpoint
       where THandler : IJsonResponseWithUrlParametersEndpointHandler<TUrlParameterModel, TResponse>
       where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        private readonly THandler handler;

        public JsonResponseWithUrlParametersEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequest(EndpointContext endpointContext)
        {
            var parameters = new TUrlParameterModel();
            parameters.BindUrlParameters(endpointContext.Request);
            if (!parameters.IsModelValid())
            {
                await HttpContextJsonHelper.WriteJsonResponse(endpointContext, parameters.Errors, 400).ConfigureAwait(false);
                return;
            }
            var response = await handler.Handle(parameters, endpointContext.RequestAborted).ConfigureAwait(false);
            await HttpContextJsonHelper.WriteJsonResponse(endpointContext, response).ConfigureAwait(false);
        }
    }
}
