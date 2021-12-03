using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class StringRequestHandler : IEndpointRequestHandler
    {
        private readonly ObjectEndpointMethodExecutor executor;
        protected readonly HttpContext httpContext;
        private readonly EndpointOptions options;
        private readonly ParameterArrayFactory parameterFactory;

        public StringRequestHandler(ObjectEndpointMethodExecutor executor, HttpContext httpContext, EndpointOptions options, ParameterArrayFactory parameterFactory)
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
            var result = (string)await executor.Execute(endpointHandler, parameters);
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "text/plain; charset = utf-8";
            await httpContext.Response.WriteAsync(result, System.Text.Encoding.UTF8, httpContext.RequestAborted);
        }
    }

}
