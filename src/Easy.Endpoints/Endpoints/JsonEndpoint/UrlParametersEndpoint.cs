using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class UrlParametersEndpoint<THandler, TUrlParameterModel> : IEndpoint 
        where THandler : IUrlParametersEndpointHandler<TUrlParameterModel>
        where TUrlParameterModel : notnull, UrlParameterModel, new()
    {
        private readonly THandler handler;

        public UrlParametersEndpoint(THandler handler)
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
            await handler.Handle(parameters, endpointContext.RequestAborted).ConfigureAwait(false);
            endpointContext.Response.StatusCode = 201;
        }
    }
}
