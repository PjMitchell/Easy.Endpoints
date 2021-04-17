using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    public class JsonBodyEndpoint<THandler, TBody> : IEndpoint where THandler : IJsonBodyEndpointHandler<TBody>
    {
        private readonly THandler handler;

        public JsonBodyEndpoint(THandler handler)
        {
            this.handler = handler;
        }

        public async Task HandleRequest(HttpContext httpContext)
        {
            var body = await HttpContextJsonHelper.ReadJsonBody<TBody>(httpContext).ConfigureAwait(false);
            if (body is null)
            {
                httpContext.Response.StatusCode = 400;
            }
            else
            {
                await handler.Handle(body).ConfigureAwait(false);
                httpContext.Response.StatusCode = 201;
            }
        }
    }
}
