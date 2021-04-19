using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public class JsonResponseEndpoint<THandler, TResponse> : IEndpoint where THandler : IJsonResponseEndpointHandler<TResponse>
    {
        private readonly THandler handler;

        public JsonResponseEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequest(HttpContext httpContext)
        {
            var response = await handler.Handle(httpContext.RequestAborted).ConfigureAwait(false);
            await HttpContextJsonHelper.WriteJsonResponse(httpContext, response).ConfigureAwait(false);
        }
    }
}
