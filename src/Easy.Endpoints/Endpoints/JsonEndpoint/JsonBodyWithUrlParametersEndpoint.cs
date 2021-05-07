using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class JsonBodyWithUrlParametersEndpoint<THandler, TBody, TUrlParameterModel> : IEndpoint
        where THandler : IJsonBodyWithUrlParametersEndpointHandler<TBody, TUrlParameterModel>
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        private readonly THandler handler;

        public JsonBodyWithUrlParametersEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequest(EndpointContext endpointContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(endpointContext).ConfigureAwait(false);
            var parameters = new TUrlParameterModel();
            parameters.BindUrlParameters(endpointContext.Request);
            if (!parameters.IsModelValid())
            {
                await HttpContextJsonHelper.WriteJsonResponse(endpointContext, parameters.Errors, 400).ConfigureAwait(false);
                return;
            }
            await handler.Handle(body, parameters, endpointContext.RequestAborted).ConfigureAwait(false); 
            endpointContext.Response.StatusCode = 201;
        }
    }
}
