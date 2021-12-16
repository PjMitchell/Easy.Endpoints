using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Easy.Endpoints
{
    internal class StringRequestHandler : IEndpointRequestHandler
    {
        private readonly ObjectEndpointMethodExecutor executor;
        protected readonly HttpContext httpContext;
        private readonly ParameterArrayFactory parameterFactory;

        public StringRequestHandler(ObjectEndpointMethodExecutor executor, HttpContext httpContext, ParameterArrayFactory parameterFactory)
        {
            this.executor = executor;
            this.httpContext = httpContext;
            this.parameterFactory = parameterFactory;
        }


        public async Task HandleRequest()
        {
            var options = httpContext.RequestServices.GetRequiredService<EndpointOptions>();
            var parameters = await parameterFactory(httpContext, options);
            var endpointHandler = httpContext.RequestServices.GetRequiredService(executor.EndpointType);
            var result = (string)await executor.Execute(endpointHandler, parameters);
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "text/plain; charset = utf-8";
            await httpContext.Response.WriteAsync(result, System.Text.Encoding.UTF8, httpContext.RequestAborted);
        }
    }

}
