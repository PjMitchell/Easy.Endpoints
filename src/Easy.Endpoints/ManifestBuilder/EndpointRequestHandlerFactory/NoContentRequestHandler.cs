using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class NoContentRequestHandler : IEndpointRequestHandler
    {
        private readonly NoResultEndpointMethodExecutor executor;
        protected readonly HttpContext httpContext;
        private readonly ParameterArrayFactory parameterFactory;

        public NoContentRequestHandler(NoResultEndpointMethodExecutor executor, HttpContext httpContext, ParameterArrayFactory parameterFactory)
        {
            this.executor = executor;
            this.httpContext = httpContext;
            this.parameterFactory = parameterFactory;
        }


        public async Task HandleRequest()
        {

            var parameters = await parameterFactory(httpContext);
            var endpointHandler = httpContext.RequestServices.GetRequiredService(executor.EndpointType);
            await executor.Execute(endpointHandler, parameters);
            httpContext.Response.StatusCode = 201;

        }
    }

}
