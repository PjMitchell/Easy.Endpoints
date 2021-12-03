using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class NoContentRequestHandler : IEndpointRequestHandler
    {
        private readonly NoResultEndpointMethodExecutor executor;
        protected readonly HttpContext httpContext;
        private readonly EndpointOptions options;
        private readonly ParameterArrayFactory parameterFactory;

        public NoContentRequestHandler(NoResultEndpointMethodExecutor executor, HttpContext httpContext, EndpointOptions options, ParameterArrayFactory parameterFactory)
        {
            this.executor = executor;
            this.httpContext = httpContext;
            this.options = options;
            this.parameterFactory = parameterFactory;
        }


        public async Task HandleRequest()
        {

            var parameters = await parameterFactory(httpContext, options);
            var endpointHandler = httpContext.RequestServices.GetRequiredService(executor.EndpointType);
            await executor.Execute(endpointHandler, parameters);
            httpContext.Response.StatusCode = 201;

        }
    }

}
